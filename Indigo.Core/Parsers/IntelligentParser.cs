namespace Indigo.Core.Parsers
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Threading.Tasks;

    using Indigo.Core.Lemmatization;

    public class IntelligentParser : IParser
    {
        public Task<IEnumerable<String>> ExtractDocumentWordsAsync(String filePath)
        {
            throw new System.NotImplementedException();
        }

        public async Task<IEnumerable<TextWord>> ExtractDocumentWordsWithPositionsAsync(String filePath)
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

            IEnumerable<TextWord> words = await Task.Run(() =>
            {
                List<TextWord> textWords = new List<TextWord>();

                // Get all symbols that not part of word - stop symbols
                String originalDocumentText = File.ReadAllText(filePath, Encoding.UTF8).ToLower();
                List<LemmaGenLemmatizer.StopSymbolPosition> stopSymbolPositions = new List<LemmaGenLemmatizer.StopSymbolPosition>();
                for (int i = 0; i < originalDocumentText.Length; i++)
                {
                    if (!Regex.IsMatch(originalDocumentText[i].ToString(), @"[а-яА-Я0-9]+"))
                    {
                        stopSymbolPositions.Add(new LemmaGenLemmatizer.StopSymbolPosition
                        {
                            Symbol = originalDocumentText[i],
                            Position = i
                        });
                    }
                }

                // Parse text to words
                Char[] stopSymbols = stopSymbolPositions.Select(x => x.Symbol).Distinct().ToArray();
                String[] originalDocumentWords = originalDocumentText.Split(stopSymbols, StringSplitOptions.RemoveEmptyEntries);

                // Get start index for each word
                stopSymbolPositions.Reverse();
                var stopSymbolPositionsStack = new Stack<LemmaGenLemmatizer.StopSymbolPosition>(stopSymbolPositions);

                LemmaGenLemmatizer.StopSymbolPosition stopSymbolPosition = null;
                if (stopSymbolPositionsStack.Peek().Position == 0)
                {
                    do
                    {
                        stopSymbolPosition = stopSymbolPositionsStack.Pop();
                    } while (stopSymbolPositionsStack.Count > 0 &&
                        stopSymbolPosition.Position == stopSymbolPositionsStack.Peek().Position - 1);
                }

                foreach (String originalDocumentWord in originalDocumentWords)
                {
                    TextWord textWord = new TextWord
                    {
                        Word = originalDocumentWord,
                        StartIndex = stopSymbolPosition != null ? stopSymbolPosition.Position + 1 : 0
                    };
                    textWords.Add(textWord);

                    do
                    {
                        stopSymbolPosition = stopSymbolPositionsStack.Pop();
                    } while (stopSymbolPositionsStack.Count > 0 &&
                        stopSymbolPosition.Position == stopSymbolPositionsStack.Peek().Position - 1);
                }

                return textWords;
            });

            return words;
        }
    }
}