namespace Indigo.BusinessLogicLayer.Document
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;

    using Indigo.DataAccessLayer.IRepositories;
    using Indigo.DataAccessLayer.Repositories;

    public class StopWordList : ReadOnlyCollection<StopWordList.StopWordItem>
    {
         public class StopWordItem
         {
             public Int32 StopWordId { get; set; }

             public String Content { get; set; }
         }

        private StopWordList(IList<StopWordItem> list) : base(list)
        {
        }

        public static async Task<StopWordList> GetAllStopWordsAsync()
        {
            using (IStopWordsRepository stopWordsRepository = new StopWordsRepository())
            {
                var dataStopWords = await stopWordsRepository.GetAllAsync();
                List<StopWordItem> stopWords = new List<StopWordItem>();
                if (dataStopWords.Any())
                {
                    stopWords = dataStopWords.Select(x => new StopWordItem
                    {
                        StopWordId = x.StopWordId,
                        Content = x.Content
                    }).ToList();
                }

                StopWordList stopWordList = new StopWordList(stopWords);

                return stopWordList;
            }
        }
    }
}