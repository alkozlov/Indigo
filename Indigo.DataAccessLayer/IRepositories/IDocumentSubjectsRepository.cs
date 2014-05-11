namespace Indigo.DataAccessLayer.IRepositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Indigo.DataAccessLayer.Models;

    public interface IDocumentSubjectsRepository : IDisposable
    {
        Task<DocumentSubject> CreateAsync(DocumentSubject documentSubject);

        Task<IEnumerable<DocumentSubject>> GetAsync(Int32 documentId);
        Task<DocumentSubject> GetAsync(Int32 documentId, Int32 subjectId);

        Task DeleteAsync(Int32 documentId, Int32 subjectId);
        Task DeleteByDocumentIdAsync(Int32 documentId);
        Task DeleteBySubjecIdAsync(Int32 subjectId);
    }
}