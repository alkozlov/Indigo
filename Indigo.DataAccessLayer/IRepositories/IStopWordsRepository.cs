namespace Indigo.DataAccessLayer.IRepositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Indigo.DataAccessLayer.Models;

    public interface IStopWordsRepository : IDisposable
    {
        Task<StopWord> CreateAsync(StopWord stopWord);

        Task<StopWord> GetAsync(Int32 stopWordId);
        Task<StopWord> GetAsync(String stopWordContent);
        Task<IEnumerable<StopWord>> GetAllAsync();

        Task DeleteAsync(Int32 stopWordId);
    }
}