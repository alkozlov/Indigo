namespace Indigo.DataAccessLayer.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Core;
    using System.Linq;
    using System.Threading.Tasks;

    using Indigo.DataAccessLayer.IRepositories;
    using Indigo.DataAccessLayer.Models;

    public class SubjectKeyWordsRepository : BaseRepository, ISubjectKeyWordsRepository
    {
        public void Dispose()
        {
            
        }

        public async Task<SubjectKeyWord> CreateAsync(SubjectKeyWord subjectKeyWord)
        {
            if (base.DataContext != null)
            {
                base.DataContext.SubjectKeyWords.Add(subjectKeyWord);
                await base.DataContext.SaveChangesAsync();

                return subjectKeyWord;
            }

            throw new EntitySqlException("Database not accessible.");
        }

        public async Task<SubjectKeyWord> GetAsync(Int32 subjectKeyWordId)
        {
            if (base.DataContext != null)
            {
                SubjectKeyWord subjectKeyWords =
                    await base.DataContext.SubjectKeyWords.FirstOrDefaultAsync(
                        x => x.SubjectKeyWordId.Equals(subjectKeyWordId));

                return subjectKeyWords;
            }

            throw new EntitySqlException("Database not accessible.");
        }

        public async Task<IEnumerable<SubjectKeyWord>> GetBySubjectIdAsync(Int32 subjectId)
        {
            if (base.DataContext != null)
            {
                var subjectKeyWords =
                    await base.DataContext.SubjectKeyWords.Where(x => x.SubjectId.Equals(subjectId)).ToListAsync();

                return subjectKeyWords;
            }

            throw new EntitySqlException("Database not accessible.");
        }

        public async Task<SubjectKeyWord> GetBySubjectIdAsync(Int32 subjectId, String word)
        {
            if (base.DataContext != null)
            {
                SubjectKeyWord subjectKeyWords =
                    await base.DataContext.SubjectKeyWords.FirstOrDefaultAsync(
                        x => x.SubjectId.Equals(subjectId) && x.Word.Equals(word));

                return subjectKeyWords;
            }

            throw new EntitySqlException("Database not accessible.");
        }

        public async Task<IEnumerable<SubjectKeyWord>> GetAllAsync()
        {
            if (base.DataContext != null)
            {
                var subjectKeyWords = await base.DataContext.SubjectKeyWords.ToListAsync();

                return subjectKeyWords;
            }

            throw new EntitySqlException("Database not accessible.");
        }

        public async Task DeleteByIdAsync(Int32 subjectKeyWordId)
        {
            if (base.DataContext != null)
            {
                var subjectKeyWord =
                    await base.DataContext.SubjectKeyWords.FirstOrDefaultAsync(x => x.SubjectKeyWordId.Equals(subjectKeyWordId));
                if (subjectKeyWord != null)
                {
                    base.DataContext.SubjectKeyWords.Remove(subjectKeyWord);
                    await base.DataContext.SaveChangesAsync();
                }

                return;
            }

            throw new EntitySqlException("Database not accessible.");
        }

        public async Task DeleteBySubjectIdAsync(Int32 subjectId)
        {
            if (base.DataContext != null)
            {
                var subjectKeyWords =
                    base.DataContext.SubjectKeyWords.Where(x => x.SubjectId.Equals(subjectId));
                if (subjectKeyWords.Any())
                {
                    base.DataContext.SubjectKeyWords.RemoveRange(subjectKeyWords);
                    await base.DataContext.SaveChangesAsync();
                }
                
                return;
            }

            throw new EntitySqlException("Database not accessible.");
        }

        public async Task DeleteRangeAsync(IEnumerable<Int32> subjectKeyWordIds)
        {
            if (base.DataContext != null)
            {
                var subjectKeyWords =
                    base.DataContext.SubjectKeyWords.Where(x => subjectKeyWordIds.Contains(x.SubjectId));
                if (subjectKeyWords.Any())
                {
                    base.DataContext.SubjectKeyWords.RemoveRange(subjectKeyWords);
                    await base.DataContext.SaveChangesAsync();
                }

                return;
            }

            throw new EntitySqlException("Database not accessible.");
        }
    }
}