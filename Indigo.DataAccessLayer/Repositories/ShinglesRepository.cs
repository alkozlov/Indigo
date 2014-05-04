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

    public class ShinglesRepository : BaseRepository, IShinglesRepository
    {
        public void Dispose()
        {
            
        }

        #region Create

        public async Task<IEnumerable<Shingle>> CreateShinglesAsync(List<Shingle> shingles)
        {
            if (base.DataContext != null)
            {
                base.DataContext.Shingles.AddRange(shingles);
                await base.DataContext.SaveChangesAsync();

                return shingles;
            }

            throw new EntitySqlException("Database not accessible.");
        }

        #endregion

        #region Get

        public async Task<IEnumerable<Shingle>> GetShinglesAsync(Int32 documentId)
        {
            if (base.DataContext != null)
            {
                var shingles = await Task.Run(() => base.DataContext.Shingles.Where(x => x.DocumentId.Equals(documentId)));

                return shingles;
            }

            throw new EntitySqlException("Database not accessible.");
        }

        public async Task<IEnumerable<Shingle>> GetShinglesAsync(Int32 documentId, Byte shingleSize)
        {
            if (base.DataContext != null)
            {
                var shingles = await Task.Run(() => base.DataContext.Shingles.Where(x => x.DocumentId.Equals(documentId) && x.ShingleSize.Equals(shingleSize)));

                return shingles;
            }

            throw new EntitySqlException("Database not accessible.");
        }

        public async Task<Int32> GetShinglesCountAsync(Int32 documentId, Byte shingleSize)
        {
            if (base.DataContext != null)
            {
                Int32 shinglesCount =
                    await base.DataContext.Shingles.Where(
                        x => x.DocumentId.Equals(documentId) && x.ShingleSize.Equals(shingleSize)).CountAsync();

                return shinglesCount;
            }

            throw new EntitySqlException("Database not accessible.");
        }

        #endregion

        #region Delete

        public async Task DeleteAsync(long shingleId)
        {
            if (base.DataContext != null)
            {
                Shingle shingle = await base.DataContext.Shingles.FirstOrDefaultAsync(x => x.ShingleId.Equals(shingleId));
                if (shingle != null)
                {
                    base.DataContext.Shingles.Remove(shingle);
                    await base.DataContext.SaveChangesAsync();
                }

                return;
            }

            throw new EntitySqlException("Database not accessible.");
        }

        public async Task DeleteAllAsync(List<long> shingleIds)
        {
            if (base.DataContext != null)
            {
                var shingles = base.DataContext.Shingles.Where(x => shingleIds.Contains(x.ShingleId));
                base.DataContext.Shingles.RemoveRange(shingles);
                await base.DataContext.SaveChangesAsync();

                return;
            }

            throw new EntitySqlException("Database not accessible.");
        }

        #endregion
    }
}