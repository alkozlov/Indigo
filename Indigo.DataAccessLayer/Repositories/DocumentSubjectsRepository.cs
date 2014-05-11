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

    public class DocumentSubjectsRepository : BaseRepository, IDocumentSubjectsRepository
    {
        public void Dispose()
        {
            
        }

        public async Task<DocumentSubject> CreateAsync(DocumentSubject documentSubject)
        {
            if (base.DataContext != null)
            {
                base.DataContext.DocumentSubjects.Add(documentSubject);
                await base.DataContext.SaveChangesAsync();

                return documentSubject;
            }

            throw new EntitySqlException("Database not accessible.");
        }

        public async Task<IEnumerable<DocumentSubject>> GetAsync(Int32 documentId)
        {
            if (base.DataContext != null)
            {
                List<DocumentSubject> documentSubjects =
                    await base.DataContext.DocumentSubjects.Where(x => x.DocumentId.Equals(documentId)).ToListAsync();

                return documentSubjects;
            }

            throw new EntitySqlException("Database not accessible.");
        }

        public async Task<DocumentSubject> GetAsync(Int32 documentId, Int32 subjectId)
        {
            if (base.DataContext != null)
            {
                DocumentSubject documentSubject =
                    await base.DataContext.DocumentSubjects.FirstOrDefaultAsync(
                        x => x.DocumentId.Equals(documentId) && x.SubjectId.Equals(subjectId));

                return documentSubject;
            }

            throw new EntitySqlException("Database not accessible.");
        }

        public async Task DeleteAsync(Int32 documentId, Int32 subjectId)
        {
            if (base.DataContext != null)
            {
                DocumentSubject documentSubject =
                    await base.DataContext.DocumentSubjects.FirstOrDefaultAsync(
                        x => x.DocumentId.Equals(documentId) && x.SubjectId.Equals(subjectId));

                if (documentSubject != null)
                {
                    base.DataContext.DocumentSubjects.Remove(documentSubject);
                    await base.DataContext.SaveChangesAsync();
                }

                return;
            }

            throw new EntitySqlException("Database not accessible.");
        }

        public async Task DeleteByDocumentIdAsync(Int32 documentId)
        {
            if (base.DataContext != null)
            {
                List<DocumentSubject> documentSubjects =
                    await base.DataContext.DocumentSubjects.Where(x => x.DocumentId.Equals(documentId)).ToListAsync();

                if (documentSubjects.Any())
                {
                    base.DataContext.DocumentSubjects.RemoveRange(documentSubjects);
                    await base.DataContext.SaveChangesAsync();
                }

                return;
            }

            throw new EntitySqlException("Database not accessible.");
        }

        public async Task DeleteBySubjecIdAsync(Int32 subjectId)
        {
            if (base.DataContext != null)
            {
                List<DocumentSubject> documentSubjects =
                    await base.DataContext.DocumentSubjects.Where(x => x.SubjectId.Equals(subjectId)).ToListAsync();

                if (documentSubjects.Any())
                {
                    base.DataContext.DocumentSubjects.RemoveRange(documentSubjects);
                    await base.DataContext.SaveChangesAsync();
                }

                return;
            }

            throw new EntitySqlException("Database not accessible.");
        }
    }
}