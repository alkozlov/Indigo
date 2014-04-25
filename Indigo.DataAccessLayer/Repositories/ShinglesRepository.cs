namespace Indigo.DataAccessLayer.Repositories
{
    using System;
    using System.Collections.Generic;
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

        #endregion
    }
}