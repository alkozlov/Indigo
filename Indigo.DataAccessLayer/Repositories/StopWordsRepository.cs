namespace Indigo.DataAccessLayer.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Data.Entity;
    using System.Data.Entity.Core;
    using System.Threading.Tasks;

    using Indigo.DataAccessLayer.IRepositories;
    using Indigo.DataAccessLayer.Models;

    public class StopWordsRepository : BaseRepository, IStopWordsRepository
    {
        public void Dispose()
        {
            
        }

        #region Create

        public async Task<StopWord> CreateAsync(StopWord stopWord)
        {
            if (base.DataContext != null)
            {
                base.DataContext.StopWords.Add(stopWord);
                await base.DataContext.SaveChangesAsync();

                return stopWord;
            }

            throw new EntitySqlException("Database not accessible.");
        }

        #endregion

        #region Get

        public async Task<StopWord> GetAsync(Int32 stopWordId)
        {
            if (base.DataContext != null)
            {
                StopWord stopWord = await base.DataContext.StopWords.FirstOrDefaultAsync(x => x.StopWordId.Equals(stopWordId));

                return stopWord;
            }

            throw new EntitySqlException("Database not accessible.");
        }

        public async Task<StopWord> GetAsync(String stopWordContent)
        {
            if (base.DataContext != null)
            {
                StopWord stopWord = await base.DataContext.StopWords.FirstOrDefaultAsync(x => x.Content.Equals(stopWordContent));

                return stopWord;
            }

            throw new EntitySqlException("Database not accessible.");
        }

        public async Task<IEnumerable<StopWord>> GetAllAsync()
        {
            List<StopWord> stopWords = await base.DataContext.StopWords.ToListAsync();

            return stopWords;
        }

        #endregion

        #region Delete

        public async Task DeleteAsync(Int32 stopWordId)
        {
            if (base.DataContext != null)
            {
                StopWord stopWord = await base.DataContext.StopWords.FirstOrDefaultAsync(x => x.StopWordId.Equals(stopWordId));
                if (stopWord != null)
                {
                    base.DataContext.StopWords.Remove(stopWord);
                    await base.DataContext.SaveChangesAsync();

                    return;
                }
            }

            throw new EntitySqlException("Database not accessible.");
        }

        #endregion
    }
}