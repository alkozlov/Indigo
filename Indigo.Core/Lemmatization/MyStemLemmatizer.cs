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

        public async Task LemmatizationDocumentAsync(String originalDocument, String outputDocument)
        {
            await Task.Run(() =>
            {
                FileInfo fileInfo = new FileInfo(originalDocument);
                if (!fileInfo.Exists)
                {
                    String exceptionMessage = String.Format("File {0} not found.", originalDocument);
                    throw new FileNotFoundException(exceptionMessage, fileInfo.FullName);
                }

                if (fileInfo.Length <= 0)
                {
                    throw new EmptyFileException("File is empty.", originalDocument);
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
                lemmatizerProcess.StartInfo.Arguments = this.GetDefaultArgumentsString(originalDocument);
                lemmatizerProcess.StartInfo.UseShellExecute = false;
                lemmatizerProcess.StartInfo.RedirectStandardInput = true;
                lemmatizerProcess.StartInfo.RedirectStandardOutput = true;
                lemmatizerProcess.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;

                lemmatizerProcess.Start();
                StreamReader outputStream = lemmatizerProcess.StandardOutput;
                String outputStreamContent = outputStream.ReadToEnd();
                lemmatizerProcess.WaitForExit();
                lemmatizerProcess.Close();

                File.WriteAllText(outputDocument, outputStreamContent, Encoding.Default);
            });
        }

        #region Constructors

        public MyStemLemmatizer(String myStemLemmatizerTool)
        {
            this.MyStemLemmatizerTool = myStemLemmatizerTool;
        }

        #endregion

        #region Helpers

        private String GetDefaultArgumentsString(String originalDocument)
        {
            StringBuilder argumentsStringBuilder = new StringBuilder("-");

            argumentsStringBuilder.Append("n");

            // Only Lemmas And Grammemes
            argumentsStringBuilder.Append("l");

            // Document punctuation
            //argumentsStringBuilder.Append("c");

            //argumentsStringBuilder.AppendFormat(" {0} {1}", originalDocument, outputDocument);
            argumentsStringBuilder.AppendFormat(" -e utf-8 {0} {1}", originalDocument, "-");

            return argumentsStringBuilder.ToString();
        }

        #endregion
    }
}