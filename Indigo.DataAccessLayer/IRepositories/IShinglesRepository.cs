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
    }
}