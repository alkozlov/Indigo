namespace Indigo.WinClient.Helpers
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Indigo.BusinessLogicLayer.Account;
    using Indigo.BusinessLogicLayer.Document;
    using Indigo.BusinessLogicLayer.Shingles;
    using Indigo.Tools;
    using Indigo.Tools.Converters;
    using Indigo.Tools.Parsers;

    public class DocumentProcessor
    {
        private const String DefaultTempApplicationFolder = @"Indigo\\";
        private const String DefaultTempLematizationFilePostfix = "_LEM";

        #region Singltone

        private static DocumentProcessor _current;

        public static DocumentProcessor Current
        {
            get { return _current ?? (_current = new DocumentProcessor()); }
        }

        #endregion

        #region Constructors

        private DocumentProcessor()
        {
            this.Configurations = new DocumentProcessorConfigurations
            {
                TempApplicationFolder = String.Concat(Path.GetTempPath(), "\\", DefaultTempApplicationFolder),
                TempLematizationFilePostfix = DefaultTempLematizationFilePostfix
            };
        }

        #endregion

        #region Properties

        public DocumentProcessorConfigurations Configurations { get; private set; }

        #endregion

        public async Task ProcessDocumentsAsync(List<String> files)
        {
            // Get existing files from input files
            List<String> existingFiles =
                files.Select(file => new FileInfo(file))
                    .Where(fileInfo => fileInfo.Exists)
                    .Select(fileInfo => fileInfo.FullName)
                    .ToList();

            if (existingFiles.Count == 0)
            {
                return;
            }

            await Task.Run(() => existingFiles.AsParallel().ForAll(ProcessFile));
        }

        private async void ProcessFile(String file)
        {
            Stopwatch stopwatch = new Stopwatch();
            String tempFileOnlyName = Guid.NewGuid().ToString("N");
            String tempTextFileName = String.Format("{0}.txt", tempFileOnlyName);
            String tempTextFileFullName = String.Concat(this.Configurations.TempApplicationFolder, tempTextFileName);

            #region Shigles algoritm

            // 1. Add new docuent to database and storage
            Document storedDocument = await Document.CreateAsync(file, IndigoUserPrincipal.Current.Identity.User);

            // 2. Convert original document to txt format and create copy in temp folder
            using (IDocumentConverter converter = new MsWordDocumentConverter())
            {
                await converter.ConvertDocumentToTextFileAsync(file, tempTextFileFullName);
            }

            try
            {
                Debug.WriteLine("===========> D{0}: Shingle algorithm begin.", storedDocument.DocumentId);
                stopwatch.Reset();
                stopwatch.Start();

                // 3. Start shingles algorithm
                String tempLematizationFileName = String.Format("{0}{1}.txt", tempFileOnlyName,
                    this.Configurations.TempLematizationFilePostfix);
                String tempLematizationFileFullName = String.Concat(this.Configurations.TempApplicationFolder,
                    tempLematizationFileName);
                await LematizationTool.Current.ProcessDocumntAsync(tempTextFileFullName, tempLematizationFileFullName);

                // Parse temp documents to words
                var words = (await ParserTool.Current.ParseFileAsync(tempLematizationFileFullName)).ToList();

                // TODO: get stop words from database
                List<String> stopWords = new List<String>();

                foreach (var shihgleSize in this.Configurations.ShihgleSizes)
                {
                    // Build shingle list for each shingle size
                    ShingleList shingles = await ShingleList.CreateAsync(words, stopWords, shihgleSize, storedDocument.DocumentId);

                    // Calculate check sum for shingles and save them to database
                    CheckSumCollection checkSumCollection = await CheckSumCollection.CreateAsync(shingles, HashAlgorithmType.SHA256);
                    await checkSumCollection.SaveAsync();
                }

                stopwatch.Stop();
                Debug.WriteLine("===========> D{0}: Shingle algorithm complete. Execiting time - {1} ms.", storedDocument.DocumentId, stopwatch.ElapsedMilliseconds);
                // ====> End shingles algorithm
            }
            catch (Exception e)
            {
                Debug.WriteLine("Error{0}: {1}", e.HResult, e.Message);
            }
            finally
            {
                // Remove temp file
                File.Delete(tempTextFileFullName);
            }

            #endregion
        }
    }
}