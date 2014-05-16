namespace Indigo.Tools.Converters
{
    using System;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;

    using Code7248.word_reader;

    public class Code7248DocumentConverter : IDocumentConverter
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

                TextExtractor textExtractor = new TextExtractor(originDocumentName);
                String documentContent = textExtractor.ExtractText();
                File.WriteAllText(outputFileName, documentContent, Encoding.UTF8);
            });
        }

        public Task<string> ConvertDocumentToText(string originDocumentName)
        {
            throw new NotImplementedException();
        }
    }
}