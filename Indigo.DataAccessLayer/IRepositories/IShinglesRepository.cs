namespace Indigo.DataAccessLayer.IRepositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Indigo.DataAccessLayer.Models;

    public interface IShinglesRepository : IDisposable
    {
        Task<IEnumerable<Shingle>> CreateShinglesAsync(List<Shingle> shingles);

        Task<IEnumerable<Shingle>> GetShinglesAsync(Int32 documentId);
        Task<IEnumerable<Shingle>> GetShinglesAsync(Int32 documentId, Byte shingleSize);
        Task<Int32> GetShinglesCountAsync(Int32 documentId, Byte shingleSize);
        
        Task DeleteAsync(long shingleId);
        Task DeleteAllAsync(List<long> shingleIds);
    }
}