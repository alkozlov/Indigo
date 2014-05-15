namespace Indigo.Tools.Converters
{
    using System;
    using System.IO;
    using System.Threading.Tasks;

    using Microsoft.Office.Core;
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
                object convertedDocumentPath = outputFileName;
                object convertedDocumentFormat = MsWord.WdSaveFormat.wdFormatText;
                object convertedDocumentEncoding = MsoEncoding.msoEncodingUTF8;

                MsWord.Document document = application.Documents.Open(ref documentPath, ref missing, ref readOnly,
                    ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref missing,
                    ref missing, ref missing, ref missing, ref missing, ref missing, ref missing);

                document.SaveAs(ref convertedDocumentPath, ref convertedDocumentFormat, ref missing, ref missing, ref missing, 
                    ref missing, ref missing, ref missing, ref missing, ref missing, ref missing, ref convertedDocumentEncoding,
                    ref missing, ref missing, ref missing, ref missing);
                document.Close();
                application.Quit();
            });
        }
    }
}