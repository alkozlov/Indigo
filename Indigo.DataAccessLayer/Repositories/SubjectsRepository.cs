namespace Indigo.DataAccessLayer.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Core;
    using System.Threading.Tasks;

    using Indigo.DataAccessLayer.IRepositories;
    using Indigo.DataAccessLayer.Models;

    public class SubjectsRepository : BaseRepository, ISubjectsRepository
    {
        public void Dispose()
        {
            
        }

        #region Create

        public async Task<Subject> CreateAsync(Subject subject)
        {
            if (base.DataContext != null)
            {
                base.DataContext.Subjects.Add(subject);
                await base.DataContext.SaveChangesAsync();

                return subject;
            }

            throw new EntitySqlException("Database not accessible.");
        }

        #endregion

        #region Get

        public async Task<Subject> GetAsync(Int32 subjectId)
        {
            if (base.DataContext != null)
            {
                Subject subject = await base.DataContext.Subjects.FirstOrDefaultAsync(x => x.SubjectId.Equals(subjectId));

                return subject;
            }

            throw new EntitySqlException("Database not accessible.");
        }

        public async Task<Subject> GetAsync(String subjectHeader)
        {
            if (base.DataContext != null)
            {
                Subject subject = await base.DataContext.Subjects.FirstOrDefaultAsync(x => x.SubjectHeader.Equals(subjectHeader));

                return subject;
            }

            throw new EntitySqlException("Database not accessible.");
        }

        public async Task<IEnumerable<Subject>> GetAllAsync()
        {
            if (base.DataContext != null)
            {
                List<Subject> subjects = await base.DataContext.Subjects.ToListAsync();

                return subjects;
            }

            throw new EntitySqlException("Database not accessible.");
        }

        #endregion

        #region Delete

        public async Task DeleteAsync(Int32 subjectId)
        {
            if (base.DataContext != null)
            {
                Subject subject = await base.DataContext.Subjects.FirstOrDefaultAsync(x => x.SubjectId.Equals(subjectId));
                if (subject != null)
                {
                    base.DataContext.Subjects.Remove(subject);
                    await base.DataContext.SaveChangesAsync();

                    return;
                }
            }

            throw new EntitySqlException("Database not accessible.");
        }

        #endregion
    }
}