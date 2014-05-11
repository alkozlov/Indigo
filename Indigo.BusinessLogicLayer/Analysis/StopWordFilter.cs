namespace Indigo.BusinessLogicLayer.Analysis
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using Indigo.BusinessLogicLayer.Document;

    public class StopWordFilter
    {
        public static async Task<List<String>> FilterAsync(List<String> words)
        {
            StopWordList stopWordList = await StopWordList.GetAllStopWordsAsync();

            // Remove stop-words from origin text
            List<String> modifiedWords = words.Select(x => x.ToLower()).ToList();
            foreach (var stopWord in stopWordList)
            {
                modifiedWords.RemoveAll(word => String.Equals(word, stopWord.Content, StringComparison.CurrentCultureIgnoreCase));
            }

            return modifiedWords;
        }
    }
}