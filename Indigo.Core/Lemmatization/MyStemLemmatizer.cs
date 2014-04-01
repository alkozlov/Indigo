namespace Indigo.Core.Lemmatization
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;

    public class MyStemLemmatizer : ILemmatizer
    {
        public String MyStemLemmatizerTool { get; private set; }

        public Task LemmatizationDocumentAsync(String origoinalDocument, String outputDocument)
        {
            Task asyncTask = Task.Factory.StartNew(() =>
            {
                FileInfo fileInfo = new FileInfo(origoinalDocument);
                if (!fileInfo.Exists)
                {
                    String exceptionMessage = String.Format("File {0} not found.", origoinalDocument);
                    throw new FileNotFoundException(exceptionMessage, fileInfo.FullName);
                }

                if (fileInfo.Length <= 0)
                {
                    throw new EmptyFileException("File is empty.", origoinalDocument);
                }

                // Verify lemmatization tool is existss
                FileInfo lemmatizerToolFileInfo = new FileInfo(this.MyStemLemmatizerTool);
                if (!lemmatizerToolFileInfo.Exists)
                {
                    String exceptionMessage = "Lemmatizer tool file 'mystem' not found.";
                    throw new FileNotFoundException(exceptionMessage, lemmatizerToolFileInfo.FullName);
                }

                // Run mystem for get new file with lemmas
                Process lemmatizerProcess = new Process();
                lemmatizerProcess.StartInfo.FileName = lemmatizerToolFileInfo.FullName;
                lemmatizerProcess.StartInfo.Arguments = this.GetDefaultArgumentsString(origoinalDocument, outputDocument);
                lemmatizerProcess.StartInfo.UseShellExecute = false;
                lemmatizerProcess.StartInfo.RedirectStandardInput = true;
                lemmatizerProcess.StartInfo.RedirectStandardOutput = true;

                lemmatizerProcess.Start();
                lemmatizerProcess.WaitForExit();
                lemmatizerProcess.Close();
            });

            return asyncTask;
        }

        #region Constructors

        public MyStemLemmatizer(String myStemLemmatizerTool)
        {
            this.MyStemLemmatizerTool = myStemLemmatizerTool;
        }

        #endregion

        #region Helpers

        private String GetDefaultArgumentsString(String originalDocument, String outputDocument)
        {
            StringBuilder argumentsStringBuilder = new StringBuilder("-");

            // Only Lemmas And Grammemes
            argumentsStringBuilder.Append("l");

            // Document punctuation
            argumentsStringBuilder.Append("c");

            argumentsStringBuilder.AppendFormat(" {0} {1}", originalDocument, outputDocument);

            return argumentsStringBuilder.ToString();
        }

        #endregion
    }
}