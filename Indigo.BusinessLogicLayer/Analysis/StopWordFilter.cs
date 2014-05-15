namespace Indigo.BusinessLogicLayer.Analysis
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Indigo.BusinessLogicLayer.Document;

    public class StopWordFilter
    {
        public static async Task<List<DocumentWord>> FilterAsync(List<DocumentWord> words)
        {
            StopWordList stopWordList = await StopWordList.GetAllStopWordsAsync();

            // Remove stop-words from origin text
            List<DocumentWord> modifiedWords = words.Select(x => new DocumentWord
            {
                Word = x.Word.ToLower(),
                StartIndex = x.StartIndex
            }).ToList();
            foreach (var stopWord in stopWordList)
            {
                modifiedWords.RemoveAll(x => String.Equals(x.Word, stopWord.Content, StringComparison.CurrentCultureIgnoreCase));
            }

            return modifiedWords;
        }
    }
}