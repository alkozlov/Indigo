namespace Indigo.DataAccessLayer.IRepositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Indigo.DataAccessLayer.Models;

    public interface ISubjectsRepository : IDisposable
    {
        Task<Subject> CreateAsync(Subject subject);

        Task<Subject> GetAsync(Int32 subjectId);
        Task<Subject> GetAsync(String subjectHeader);
        Task<IEnumerable<Subject>> GetAllAsync();

        Task DeleteAsync(Int32 subjectId);
    }
}