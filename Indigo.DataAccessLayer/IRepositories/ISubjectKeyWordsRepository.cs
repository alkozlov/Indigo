namespace Indigo.DataAccessLayer.IRepositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Indigo.DataAccessLayer.Models;

    public interface ISubjectKeyWordsRepository : IDisposable
    {
        Task<SubjectKeyWord> CreateAsync(SubjectKeyWord subjectKeyWord);

        Task<SubjectKeyWord> GetAsync(Int32 subjectKeyWordId);
        Task<IEnumerable<SubjectKeyWord>> GetBySubjectIdAsync(Int32 subjectId);
        Task<SubjectKeyWord> GetBySubjectIdAsync(Int32 subjectId, String word);
        Task<IEnumerable<SubjectKeyWord>> GetAllAsync();

        Task DeleteByIdAsync(Int32 subjectKeyWordId);
        Task DeleteBySubjectIdAsync(Int32 subjectId);
        Task DeleteRangeAsync(IEnumerable<Int32> subjectKeyWordIds);
    }
}