using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;
using Indigo.BusinessLogicLayer.Account;
using Indigo.BusinessLogicLayer.Document;
using Indigo.BusinessLogicLayer.Shingles;
using Indigo.Tools.Converters;
using Tools = Indigo.Tools;

namespace Indigo.BusinessLogicLayer.Analysis
{
    public class DocumentProcessor
    {
        private const String DefaultTempApplicationFolder = @"Indigo\\";
        private const String DefaultTempLematizationFilePostfix = "_LEM";
        private const Int32 MinimalWordsUsageInDocument = 2;

        #region Events

        public event EventHandler<DocumentProcessingEventArgs> OnDocumentProcessing;
        public event EventHandler<DocumentProcessedEventArgs> OnDocumentProcessed;
        public event EventHandler<DocumentProcessErrorEventArgs> OnDocumentProcessError;
        public event EventHandler OnDocumentsProcessing;
        public event EventHandler OnDocumentsProcessed;

        #endregion

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

        #region Methods

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

            this.OnDocumentsProcessing(this, null);
            await Task.Run(() => existingFiles.AsParallel().ForAll(ProcessFile));
        }

        private async void ProcessFile(String file)
        {
            DocumentProcessingEventArgs documentProcessingEventArgs = new DocumentProcessingEventArgs(file);
            this.OnDocumentProcessing(this, documentProcessingEventArgs);

            Stopwatch stopwatch = new Stopwatch();
            String tempFileOnlyName = Guid.NewGuid().ToString("N");
            String tempTextFileName = String.Format("{0}.txt", tempFileOnlyName);
            String tempTextFileFullName = String.Concat(this.Configurations.TempApplicationFolder, tempTextFileName);
            String tempLematizationFileName = String.Format("{0}{1}.txt", tempFileOnlyName,
                this.Configurations.TempLematizationFilePostfix);
            String tempLematizationFileFullName = String.Concat(this.Configurations.TempApplicationFolder, tempLematizationFileName);

            try
            {
                // 1. Add new docuent to database and storage
                Document.Document storedDocument = await Document.Document.CreateAsync(file, IndigoUserPrincipal.Current.Identity.User);

                // 2. Convert original document to txt format and create copy in temp folder
                using (IDocumentConverter converter = new MsWordDocumentConverter())
                {
                    await converter.ConvertDocumentToTextFileAsync(file, tempTextFileFullName);
                }

                ExceptionDispatchInfo documentProcessingExceptionInfo = null;

                try
                {
                    Debug.WriteLine("===========> D{0}: Shingle algorithm begin.", storedDocument.DocumentId);
                    stopwatch.Reset();
                    stopwatch.Start();

                    // 3. Lematization
                    await Tools.LematizationTool.Current.ProcessDocumntAsync(tempTextFileFullName, tempLematizationFileFullName);

                    // Parse temp documents to words
                    var parserWords = (await Tools.Parsers.ParserTool.Current.ParseDocumentAsync(tempLematizationFileFullName)).ToList();
                    List<DocumentWord> documentWords = ConvertToBusinessDocumentWords(parserWords);

                    // Remove stop-words from origin text
                    List<DocumentWord> modifiedWords = await StopWordFilter.FilterAsync(documentWords);

                    #region Shingles algorithm

                    foreach (var shihgleSize in this.Configurations.ShihgleSizes)
                    {
                        // Build shingle list for each shingle size
                        ShingleList shingles =
                            await ShingleList.CreateAsync(modifiedWords, shihgleSize, storedDocument.DocumentId);

                        // Calculate check sum for shingles and save them to database
                        CheckSumCollection checkSumCollection =
                            await CheckSumCollection.CreateAsync(shingles, HashAlgorithmType.SHA256);
                        await checkSumCollection.SaveAsync();
                    }

                    stopwatch.Stop();
                    Debug.WriteLine("===========> D{0}: Shingle algorithm complete. Execiting time - {1} ms.",
                        storedDocument.DocumentId, stopwatch.ElapsedMilliseconds);
                    #endregion

                    #region LSA

                    List<DocumentWord> modifiedWordsUsageFilter = UsageFilter.Filter(modifiedWords, MinimalWordsUsageInDocument);
                    DocumentVector documentVector = DocumentVectorization.Vectorisation(modifiedWordsUsageFilter);
                    DocumentKeyWordList documentKeyWordList =
                        await DocumentKeyWordList.CreateAndSaveAsync(storedDocument.DocumentId, documentVector);

                    #endregion
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Error{0}: {1}", e.HResult, e.Message);
                    documentProcessingExceptionInfo = ExceptionDispatchInfo.Capture(e);
                }
                finally
                {
                    // Remove temp files
                    File.Delete(tempTextFileFullName);
                    File.Delete(tempLematizationFileFullName);
                }

                if (documentProcessingExceptionInfo != null)
                {
                    // Remove document from database
                    await storedDocument.DeleteAsync();
                    documentProcessingExceptionInfo.Throw();
                }

                DocumentProcessedEventArgs documentProcessedEventArgs = new DocumentProcessedEventArgs(file);
                this.OnDocumentProcessed(this, documentProcessedEventArgs);
            }
            catch (Exception e)
            {
                DocumentProcessErrorEventArgs documentProcessErrorEventArgs = new DocumentProcessErrorEventArgs(file, e);
                this.OnDocumentProcessError(this, documentProcessErrorEventArgs);
            }
        }

        #endregion

        #region Helpers

        private List<DocumentWord> ConvertToBusinessDocumentWords(List<Tools.Parsers.DocumentWord> toolDocumentWords)
        {
            List<DocumentWord> documentWords = toolDocumentWords.Select(x => new DocumentWord
            {
                Word = x.Word,
                StartIndex = x.StartIndex
            }).ToList();

            return documentWords;
        }

        #endregion
    }
}