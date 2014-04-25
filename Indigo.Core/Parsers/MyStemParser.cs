namespace Indigo.Core.Parsers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Indigo.Core.Lemmatization;

    public class MyStemParser : IParser
    {
        public async Task<IEnumerable<String>> ExtractDocumentWordsAsync(String filePath)
        {
            FileInfo fileInfo = new FileInfo(filePath);
            if (!fileInfo.Exists)
            {
                throw new FileNotFoundException("Can't find document for parsing.", filePath);
            }

            if (fileInfo.Length == 0)
            {
                throw new EmptyFileException("File for parsing is empty.", filePath);
            }

            IEnumerable<String> words = await Task.Run(() =>
            {
                List<String> fileWords = new List<String>();

                using (TextReader reader = new StreamReader(filePath))
                {
                    String line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        String[] possibleWords = line.Split(new char[] {'?', '|'}, StringSplitOptions.RemoveEmptyEntries);
                        if (possibleWords.Length > 0)
                        {
                            fileWords.Add(possibleWords.First());
                        }
                    }
                }

                return fileWords;
            });

            return words;
        }
    }
}