namespace Indigo.DataAccessLayer.IRepositories
{
    using System;
    using System.Threading.Tasks;

    using Indigo.DataAccessLayer.Models;

    public interface IUserAccountRepository
    {
        Task<UserAccount> CreateAsync(String login, String email, String password, String passwordSalt, Byte accountType);

        Task<UserAccount> GetAsync(Int32 userId);

        Task<UserAccount> GetAsync(Guid userGuid);

        Task<UserAccount> GetAsync(String emailOrLogin);

        Task<UserAccount> UpdateAsync(Int32 userId, Guid userGuid, String login, String email, String password,
            String passwordSalt, DateTime createDateUtc, DateTime lastLoginDateUtc, DateTime removedDateUtc,
            Byte accountType, Boolean isActive);

        Task DeleteAsync(Int32 userId);
    }
}