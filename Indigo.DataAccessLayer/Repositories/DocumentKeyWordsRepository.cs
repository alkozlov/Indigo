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

    public class DocumentKeyWordsRepository : BaseRepository, IDocumentKeyWordsRepository
    {
        public void Dispose()
        {

        }

        public async Task<DocumentKeyWord> CreateAsync(DocumentKeyWord documentKeyWord)
        {
            if (base.DataContext != null)
            {
                base.DataContext.DocumentKeyWords.Add(documentKeyWord);
                await base.DataContext.SaveChangesAsync();

                return documentKeyWord;
            }

            throw new EntitySqlException("Database not accessible.");
        }

        public async Task<IEnumerable<DocumentKeyWord>> CreateRangeAsync(Int32 documentId, Dictionary<String, Int32> words)
        {
            List<DocumentKeyWord> documentKeyWords = words.Select(word => new DocumentKeyWord
            {
                DocumentId = documentId,
                Word = word.Key,
                Usages = word.Value
            }).ToList();

            base.DataContext.DocumentKeyWords.AddRange(documentKeyWords);
            await base.DataContext.SaveChangesAsync();

            return documentKeyWords;
        }

        public async Task<DocumentKeyWord> GetAsync(Int64 documentKeyWordId)
        {
            if (base.DataContext != null)
            {
                DocumentKeyWord documentKeyWord =
                    await base.DataContext.DocumentKeyWords.FirstOrDefaultAsync(x => x.DocumentKeyWordId.Equals(documentKeyWordId));

                return documentKeyWord;
            }

            throw new EntitySqlException("Database not accessible.");
        }

        public async Task<IEnumerable<DocumentKeyWord>> GetByDocumentIdAsync(Int32 documentId)
        {
            if (base.DataContext != null)
            {
                var documentKeyWords =
                    await base.DataContext.DocumentKeyWords.Where(x => x.DocumentId.Equals(documentId)).ToListAsync();

                return documentKeyWords;
            }

            throw new EntitySqlException("Database not accessible.");
        }

        public async Task<DocumentKeyWord> GetByDocumentIdAsync(Int32 documentId, String word)
        {
            if (base.DataContext != null)
            {
                DocumentKeyWord documentKeyWords =
                    await base.DataContext.DocumentKeyWords.FirstOrDefaultAsync(
                        x => x.DocumentId.Equals(documentId) && x.Word.Equals(word));

                return documentKeyWords;
            }

            throw new EntitySqlException("Database not accessible.");
        }

        public async Task<IEnumerable<DocumentKeyWord>> GetAllAsync()
        {
            if (base.DataContext != null)
            {
                var documentKeyWords = await base.DataContext.DocumentKeyWords.ToListAsync();

                return documentKeyWords;
            }

            throw new EntitySqlException("Database not accessible.");
        }

        public async Task DeleteByIdAsync(Int64 documentKeyWordId)
        {
            if (base.DataContext != null)
            {
                var documentKeyWord =
                    await base.DataContext.DocumentKeyWords.FirstOrDefaultAsync(x => x.DocumentKeyWordId.Equals(documentKeyWordId));
                if (documentKeyWord != null)
                {
                    base.DataContext.DocumentKeyWords.Remove(documentKeyWord);
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
                var documentKeyWords =
                    base.DataContext.DocumentKeyWords.Where(x => x.DocumentId.Equals(documentId));
                if (documentKeyWords.Any())
                {
                    base.DataContext.DocumentKeyWords.RemoveRange(documentKeyWords);
                    await base.DataContext.SaveChangesAsync();
                }

                return;
            }

            throw new EntitySqlException("Database not accessible.");
        }

        public async Task DeleteRangeAsync(IEnumerable<Int64> documentKeyWordIds)
        {
            if (base.DataContext != null)
            {
                var documentKeyWords =
                    base.DataContext.DocumentKeyWords.Where(x => documentKeyWordIds.Contains(x.DocumentId));
                if (documentKeyWords.Any())
                {
                    base.DataContext.DocumentKeyWords.RemoveRange(documentKeyWords);
                    await base.DataContext.SaveChangesAsync();
                }

                return;
            }

            throw new EntitySqlException("Database not accessible.");
        }
    }
}