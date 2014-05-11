namespace Indigo.DataAccessLayer.IRepositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Indigo.DataAccessLayer.Models;

    public interface IDocumentsRepository : IDisposable
    {
        Task<Document> CreateAsync(Document document);
        
        Task<Document> GetByIdAsync(Int32 documentId);
        Task<Document> GetByGuidAsync(Guid documentGuid);
        Task<Document> GetByStoredNameAsync(String storedName);
        Task<IEnumerable<Document>> GetAllDocumentsAsync();

        Task DeleteDocumentAsync(Int32 documentId);
    }
}