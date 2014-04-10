namespace Indigo.DataAccessLayer.IRepositories
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    using Indigo.DataAccessLayer.Models;

    public interface IPermissionsRepository : IDisposable
    {
        Task<IEnumerable<AccountPermission>> GetAccountPermissionsAsync(Byte accountType);

        Task<AccountPermission> UpdateAccountPermissionAsync(Byte accountType, Byte permissionType, Byte accessType);
    }
}