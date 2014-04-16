namespace Indigo.DataAccessLayer.Repositories
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Core;
    using System.Threading.Tasks;

    using Indigo.DataAccessLayer.IRepositories;
    using Indigo.DataAccessLayer.Models;

    public class UserAccountRepository : BaseRepository, IUserAccountRepository
    {
        public async Task<UserAccount> CreateAsync(String login, String email, String password, String passwordSalt, Byte accountType)
        {
            if (base.DataContext != null)
            {
                UserAccount userAccount = new UserAccount
                {
                    UserGuid = Guid.NewGuid(),
                    Email = email,
                    Login = login,
                    Password = password,
                    PasswordSalt = passwordSalt,
                    CreatedDateUtc = DateTime.UtcNow,
                    LastLoginDateUtc = null,
                    RemovedDateUtc = null,
                    AccountType = accountType,
                    IsActive = true
                };

                base.DataContext.UserAccounts.Add(userAccount);
                await base.DataContext.SaveChangesAsync();

                return userAccount;
            }

            throw new EntitySqlException("Database not accessible.");
        }

        public async Task<UserAccount> GetAsync(Int32 userId)
        {
            if (base.DataContext != null)
            {
                UserAccount userAccount =
                    await base.DataContext.UserAccounts.FirstOrDefaultAsync(x => x.UserId.Equals(userId));

                return userAccount;
            }

            throw new EntitySqlException("Database not accessible.");
        }

        public async Task<UserAccount> GetAsync(Guid userGuid)
        {
            if (base.DataContext != null)
            {
                UserAccount userAccount =
                    await base.DataContext.UserAccounts.FirstOrDefaultAsync(x => x.UserGuid.Equals(userGuid));

                return userAccount;
            }

            throw new EntitySqlException("Database not accessible.");
        }

        public async Task<UserAccount> GetAsync(String emailOrLogin)
        {
            if (base.DataContext != null)
            {
                UserAccount userAccount =
                    await base.DataContext.UserAccounts.FirstOrDefaultAsync(x => x.Email.Equals(emailOrLogin) || x.Login.Equals(emailOrLogin));

                return userAccount;
            }

            throw new EntitySqlException("Database not accessible.");
        }

        public async Task<UserAccount> UpdateAsync(Int32 userId, Guid userGuid, String login, String email, String password, String passwordSalt,
            DateTime createDateUtc, DateTime? lastLoginDateUtc, DateTime? removedDateUtc, Byte accountType, Boolean isActive)
        {
            if (base.DataContext != null)
            {
                UserAccount userAccount =
                    await base.DataContext.UserAccounts.FirstOrDefaultAsync(x => x.UserId.Equals(userId));
                if (userAccount != null)
                {
                    userAccount.Email = email;
                    userAccount.Password = password;
                    userAccount.PasswordSalt = passwordSalt;
                    userAccount.LastLoginDateUtc = lastLoginDateUtc;
                    userAccount.RemovedDateUtc = removedDateUtc;
                    userAccount.AccountType = accountType;
                    userAccount.IsActive = isActive;

                    await base.DataContext.SaveChangesAsync();
                }

                return userAccount;
            }

            throw new EntitySqlException("Database not accessible.");
        }

        public async Task DeleteAsync(Int32 userId)
        {
            if (base.DataContext != null)
            {
                UserAccount userAccount =
                    await base.DataContext.UserAccounts.FirstOrDefaultAsync(x => x.UserId.Equals(userId));
                if (userAccount != null)
                {
                    base.DataContext.UserAccounts.Remove(userAccount);
                    await base.DataContext.SaveChangesAsync();
                }
            }

            throw new EntitySqlException("Database not accessible.");
        }

        public void Dispose()
        {
            
        }
    }
}