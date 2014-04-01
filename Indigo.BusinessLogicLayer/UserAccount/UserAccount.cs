namespace Indigo.BusinessLogicLayer.UserAccount
{
    using System;
    using System.Threading.Tasks;

    using DataModels = Indigo.DataAccessLayer.Models;
    using Indigo.BusinessLogicLayer.Helpers;
    using Indigo.DataAccessLayer.IRepositories;
    using Indigo.DataAccessLayer.Repositories;

    public class UserAccount
    {
        public const Int32 MaxPasswordLength = 100;

        public Int32 UserId { get; set; }
        public Guid UserGuid { get; set; }
        public String Email { get; set; }
        public String Login { get; set; }
        public String Password { get; set; }
        public String PasswordSalt { get; set; }
        public DateTime CreatedDateUtc { get; set; }
        public DateTime? LastLoginDateUtc { get; set; }
        public DateTime? RemovedDateYtc { get; set; }
        public UserAccountType AccountType { get; set; }
        public Boolean IsAcive { get; set; }

        private static readonly IUserAccountRepository UserAccountRepository = new UserAccountRepository();

        public static async Task<UserAccount> CreateAsync(String login, String email, String password, UserAccountType accountType)
        {
            String passwordSalt = PasswordHelper.GeneratePasswordSalt(UserAccount.MaxPasswordLength);
            String hashPassword = PasswordHelper.ComputePasswordHash(password, passwordSalt);

            DataModels.UserAccount user = await UserAccountRepository.CreateAsync(login, email, hashPassword, passwordSalt, (byte) accountType);

            return null;
        }

        public static async Task<UserAccount> GetUserAsync(String email)
        {
            DataModels.UserAccount dataUser = await UserAccountRepository.GetAsync(email);

            return null;
        }

        #region Helpers

        private UserAccount ConvertToBusinessObject(DataModels.UserAccount dataUserAccount)
        {
            UserAccount userAccount = new UserAccount()
            {
                UserId = dataUserAccount.UserId,
                UserGuid = dataUserAccount.UserGuid,
                Email = dataUserAccount.Email,
                Password = dataUserAccount.Password,
                PasswordSalt = dataUserAccount.PasswordSalt
            };

            return userAccount;
        }

        #endregion
    }
}