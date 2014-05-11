namespace Indigo.DataAccessLayer.IRepositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Indigo.DataAccessLayer.Models;

    public interface IDocumentKeyWordsRepository : IDisposable
    {
        Task<DocumentKeyWord> CreateAsync(DocumentKeyWord documentKeyWord);
        Task<IEnumerable<DocumentKeyWord>> CreateRangeAsync(Int32 documentId, Dictionary<String, Int32> words);

        Task<DocumentKeyWord> GetAsync(Int64 documentKeyWordId);
        Task<IEnumerable<DocumentKeyWord>> GetByDocumentIdAsync(Int32 documentId);
        Task<DocumentKeyWord> GetByDocumentIdAsync(Int32 documentId, String word);
        Task<IEnumerable<DocumentKeyWord>> GetAllAsync();

        Task DeleteByIdAsync(Int64 documentKeyWordId);
        Task DeleteByDocumentIdAsync(Int32 documentId);
        Task DeleteRangeAsync(IEnumerable<Int64> documentKeyWordIds);
    }
}