namespace Indigo.BusinessLogicLayer.Document
{
    using System;
    using System.Data;
    using System.Threading.Tasks;

    using AutoMapper;

    using DataModels = Indigo.DataAccessLayer.Models;
    using Indigo.DataAccessLayer.IRepositories;
    using Indigo.DataAccessLayer.Repositories;

    public class StopWord
    {
        public Int32 StopWordId { get; set; }

        public String Content { get; set; }

        public static async Task<StopWord> CreateAsync(String stopWordContent)
        {
            if (String.IsNullOrEmpty(stopWordContent) || String.IsNullOrEmpty(stopWordContent.Trim()))
            {
                throw new ArgumentNullException("stopWordContent", "Stop word can't be null or empty.");
            }

            String convertedStopWordContent = stopWordContent.Trim().ToLower();

            using (IStopWordsRepository stopWordsRepository = new StopWordsRepository())
            {
                var dataExistingStopWord = await stopWordsRepository.GetAsync(convertedStopWordContent);
                if (dataExistingStopWord != null)
                {
                    throw new DuplicateNameException(String.Format("Stop word '{0}' already exists.", convertedStopWordContent));
                }

                DataModels.StopWord dataStopWord = new DataModels.StopWord
                {
                    Content = convertedStopWordContent
                };

                dataStopWord = await stopWordsRepository.CreateAsync(dataStopWord);
                StopWord stopWord = ConvertToBusinessObject(dataStopWord);

                return stopWord;
            }
        }

        public static async Task<StopWord> GetAsync(Int32 stopWordId)
        {
            if (stopWordId <= 0)
            {
                throw new ArgumentException("Invalid stop word ID value.", "stopWordId");
            }

            using (IStopWordsRepository stopWordsRepository = new StopWordsRepository())
            {
                DataModels.StopWord dataStopWord = await stopWordsRepository.GetAsync(stopWordId);
                StopWord stopWord = dataStopWord != null ? ConvertToBusinessObject(dataStopWord) : null;

                return stopWord;
            } 
        }

        public static async Task<StopWord> GetAsync(String stopWordContent)
        {
            if (String.IsNullOrEmpty(stopWordContent))
            {
                throw new ArgumentException("Invalid stop word value.", "stopWordContent");
            }

            using (IStopWordsRepository stopWordsRepository = new StopWordsRepository())
            {
                DataModels.StopWord dataStopWord = await stopWordsRepository.GetAsync(stopWordContent);
                StopWord stopWord = dataStopWord != null ? ConvertToBusinessObject(dataStopWord) : null;

                return stopWord;
            }
        }

        public async Task DeleteAsync()
        {
            using (IStopWordsRepository stopWordsRepository = new StopWordsRepository())
            {
                await stopWordsRepository.DeleteAsync(this.StopWordId);
            }
        }

        #region Helpers

        private static StopWord ConvertToBusinessObject(DataModels.StopWord dataStopWord)
        {
            Mapper.CreateMap<DataModels.StopWord, StopWord>();
            StopWord stopWord = Mapper.Map<DataModels.StopWord, StopWord>(dataStopWord);

            return stopWord;
        }

        #endregion
    }
}