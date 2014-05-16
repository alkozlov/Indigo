namespace Indigo.Tools.Converters
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using DevExpress.XtraRichEdit;

    public class MsWordDocumentConverter : IDocumentConverter
    {
        private readonly Dictionary<DocumentFormat, String> _formatsDictionary = new Dictionary<DocumentFormat, String>();

        public MsWordDocumentConverter()
        {
            this.InitializeFormatDictionary();
        }

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

                using (RichEditDocumentServer richEditDocumentServer = new RichEditDocumentServer())
                {
                    DocumentFormat sourceFormat = this.GetDocumentFormat(originDocumentName);
                    FileStream inputStream = File.OpenRead(originDocumentName);
                    FileStream outputStream = File.Open(outputFileName, FileMode.Create);
                    richEditDocumentServer.LoadDocument(inputStream, sourceFormat);
                    richEditDocumentServer.SaveDocument(outputStream, DocumentFormat.PlainText);
                    inputStream.Close();
                    outputStream.Close();
                }
            });
        }

        public async Task<String> ConvertDocumentToText(String originDocumentName)
        {
            String documentPlanText = await Task.Run(() =>
            {
                FileInfo fileInfo = new FileInfo(originDocumentName);
                if (!fileInfo.Exists)
                {
                    throw new FileNotFoundException("Can't find document for convertation.", originDocumentName);
                }
                String outputDirectory = Path.GetDirectoryName(originDocumentName);
                DirectoryInfo outputDirectoryInfo = new DirectoryInfo(outputDirectory);
                if (!outputDirectoryInfo.Exists)
                {
                    outputDirectoryInfo.Create();
                }

                using (RichEditDocumentServer richEditDocumentServer = new RichEditDocumentServer())
                {
                    DocumentFormat sourceFormat = this.GetDocumentFormat(originDocumentName);
                    FileStream inputStream = File.OpenRead(originDocumentName);
                    richEditDocumentServer.LoadDocument(inputStream, sourceFormat);
                    inputStream.Close();
                    String planText = richEditDocumentServer.Text;

                    return planText;
                }
            });

            return documentPlanText;
        }

        private void InitializeFormatDictionary()
        {
            _formatsDictionary.Add(DocumentFormat.OpenXml, "docx");
            _formatsDictionary.Add(DocumentFormat.Rtf, "rtf");
            _formatsDictionary.Add(DocumentFormat.PlainText, "txt");
            _formatsDictionary.Add(DocumentFormat.Doc, "doc");
            _formatsDictionary.Add(DocumentFormat.ePub, "epub");
            _formatsDictionary.Add(DocumentFormat.OpenDocument, "odt");
            _formatsDictionary.Add(DocumentFormat.WordML, "xml");
            _formatsDictionary.Add(DocumentFormat.Html, "htm");
            _formatsDictionary.Add(DocumentFormat.Mht, "mht");
        }

        private DocumentFormat GetDocumentFormat(String fileName)
        {
            String extansion = Path.GetExtension(fileName);
            DocumentFormat documentFormat = this._formatsDictionary.First(x => extansion.ToLower().EndsWith(x.Value)).Key;

            return documentFormat;
        }
    }
}