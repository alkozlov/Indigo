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

    public class PermissionsRepository : BaseRepository, IPermissionsRepository
    {
        public Task<IEnumerable<AccountPermission>> GetAccountPermissionsAsync(Byte accountType)
        {
            if (base.DataContext != null)
            {
                Task<IEnumerable<AccountPermission>> asyncTask = Task<IEnumerable<AccountPermission>>.Factory.StartNew(() =>
                {
                    IQueryable<AccountPermission> accountPermissions =
                        base.DataContext.AccountPermissions.Where(x => x.AccountType == accountType);

                    return accountPermissions as IEnumerable<AccountPermission>;
                });

                return asyncTask;
            }

            throw new EntitySqlException("Database not accessible.");
        }

        public async Task<AccountPermission> UpdateAccountPermissionAsync(Byte accountType, Byte permissionType, Byte accessType)
        {
            if (base.DataContext != null)
            {
                AccountPermission accountPermission =
                    await base.DataContext.AccountPermissions.FirstOrDefaultAsync(
                        x => x.AccountType.Equals(accountType) && x.PermissionType.Equals(permissionType));

                if (accountPermission != null)
                {
                    accountPermission.AccessType = accessType;
                    await base.DataContext.SaveChangesAsync();
                    
                    return accountPermission;
                }

                return null;
            }

            throw new EntitySqlException("Database not accessible.");
        }

        public void Dispose()
        {

        }
    }
}