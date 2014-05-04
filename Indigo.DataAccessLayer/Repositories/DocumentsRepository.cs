namespace Indigo.DataAccessLayer.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Core;
    using System.Threading.Tasks;

    using Indigo.DataAccessLayer.IRepositories;
    using Indigo.DataAccessLayer.Models;

    public class DocumentsRepository : BaseRepository, IDocumentsRepository
    {
        public void Dispose()
        {
            
        }

        #region Create

        public async Task<Document> CreateAsync(Document document)
        {
            if (base.DataContext != null)
            {
                base.DataContext.Documents.Add(document);
                await base.DataContext.SaveChangesAsync();

                return document;
            }

            throw new EntitySqlException("Database not accessible.");
        }

        #endregion

        #region Get

        public async Task<Document> GetByIdAsync(Int32 documentId)
        {
            if (base.DataContext != null)
            {
                var document = await base.DataContext.Documents.FirstOrDefaultAsync(x => x.DocumentId.Equals(documentId));

                return document;
            }

            throw new EntitySqlException("Database not accessible.");
        }

        public async Task<Document> GetByGuid(Guid documentGuid)
        {
            if (base.DataContext != null)
            {
                var document = await base.DataContext.Documents.FirstOrDefaultAsync(x => x.DocumentGuid.Equals(documentGuid));

                return document;
            }

            throw new EntitySqlException("Database not accessible.");
        }

        public async Task<Document> GetByStoredNameAsync(String storedName)
        {
            if (base.DataContext != null)
            {
                var document = await base.DataContext.Documents.FirstOrDefaultAsync(x => x.StoredName.Equals(storedName));

                return document;
            }

            throw new EntitySqlException("Database not accessible.");
        }

        public async Task<IEnumerable<Document>> GetAllDocumentsAsync()
        {
            if (base.DataContext != null)
            {
                List<Document> documents = await base.DataContext.Documents.ToListAsync();

                return documents;
            }

            throw new EntitySqlException("Database not accessible.");
        }

        #endregion

        #region Delete

        public async Task DeleteDocumentAsync(Int32 documentId)
        {
            if (base.DataContext != null)
            {
                Document entity = await base.DataContext.Documents.FirstOrDefaultAsync(x => x.DocumentId.Equals(documentId));
                if (entity != null)
                {
                    base.DataContext.Documents.Remove(entity);
                    await base.DataContext.SaveChangesAsync();
                }

                return;
            }

            throw new EntitySqlException("Database not accessible.");
        }

        #endregion
    }
}