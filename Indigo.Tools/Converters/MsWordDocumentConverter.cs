namespace Indigo.Tools.Converters
{
    using System;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;

    using MsWord = Microsoft.Office.Interop.Word;

    public class MsWordDocumentConverter : IDocumentConverter
    {
        public void Dispose()
        {
            
        }

        public async Task ConvertDocumentToTextFileAsync(String originDocumentName, String outputFileName)
        {
            await Task.Run(() =>
            {
                FileInfo fileInfo = new FileInfo(originDocumentName);
                if (!fileInfo.Exists)
                {
                    throw new FileNotFoundException("Can't find document for convertation.", originDocumentName);
                }
                String outputDirectory = Path.GetDirectoryName(outputFileName);
                DirectoryInfo outputDirectoryInfo = new DirectoryInfo(outputDirectory);
                if (!outputDirectoryInfo.Exists)
                {
                    outputDirectoryInfo.Create();
                }

                MsWord.Application application = new MsWord.Application();
                object missing = Type.Missing;
                object readOnly = true;
                object documentPath = originDocumentName;

                MsWord.Document document = application.Documents.Open(ref documentPath, ref missing, ref readOnly,
                    ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing,
                    ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);

                String documentContent = document.Content.Text;
                File.WriteAllText(outputFileName, documentContent, Encoding.UTF8);
            });
        }
    }
}