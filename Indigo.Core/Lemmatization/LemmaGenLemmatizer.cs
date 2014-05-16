using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using LemmaSharp;

namespace Indigo.Core.Lemmatization
{
    public class LemmaGenLemmatizer : ILemmatizer
    {
        internal class StopSymbolPosition
        {
            public Char Symbol { get; set; }

            public Int32 Position { get; set; }
        }

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

                // Get all symbols that not part of word - stop symbols
                String originalDocumentText = File.ReadAllText(originalDocument, Encoding.Default).ToLower();
                List<StopSymbolPosition> stopSymbolPositions = new List<StopSymbolPosition>();
                for (int i = 0; i < originalDocumentText.Length; i++)
                {
                    if (!Regex.IsMatch(originalDocumentText[i].ToString(), @"[а-яА-Я0-9]+"))
                    {
                        stopSymbolPositions.Add(new StopSymbolPosition
                        {
                            Symbol = originalDocumentText[i],
                            Position = i
                        });
                    }
                }

                // Parse text to words
                Char[] stopSymbols = stopSymbolPositions.Select(x => x.Symbol).Distinct().ToArray();
                String[] originalDocumentWords = originalDocumentText.Split(stopSymbols, StringSplitOptions.RemoveEmptyEntries);

                // Get lemmas
                List<String> lemmatizedWords = new List<string>(originalDocumentWords.Length);
                LemmaSharp.ILemmatizer lemmatizer = new LemmatizerPrebuiltCompact(LanguagePrebuilt.Russian);
                lemmatizedWords.AddRange(originalDocumentWords.Select(word => lemmatizer.Lemmatize(word.ToLower())));

                // Create oroginal text but instead original words use lemmatized words
                stopSymbolPositions.Reverse();
                Stack<StopSymbolPosition> stopSymbolPositionsStack = new Stack<StopSymbolPosition>(stopSymbolPositions);
                StringBuilder stringBuilder = new StringBuilder();

                StopSymbolPosition stopSymbolPosition;
                if (stopSymbolPositionsStack.Peek().Position == 0)
                {
                    do
                    {
                        stopSymbolPosition = stopSymbolPositionsStack.Pop();
                        stringBuilder.Append(stopSymbolPosition.Symbol);
                    } while (stopSymbolPositionsStack.Count > 0 &&
                        stopSymbolPosition.Position == stopSymbolPositionsStack.Peek().Position - 1);
                }

                foreach (String lemmatizedWord in lemmatizedWords)
                {
                    stringBuilder.Append(lemmatizedWord);

                    do
                    {
                        stopSymbolPosition = stopSymbolPositionsStack.Pop();
                        stringBuilder.Append(stopSymbolPosition.Symbol);
                    } while (stopSymbolPositionsStack.Count > 0 &&
                        stopSymbolPosition.Position == stopSymbolPositionsStack.Peek().Position - 1);
                }

                File.WriteAllText(outputDocument, stringBuilder.ToString(), Encoding.Default);
            });
        }
    }
}