namespace Indigo.BusinessLogicLayer.Account
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;

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
        public DateTime? RemovedDateUtc { get; set; }
        public UserAccountType AccountType { get; set; }
        public Boolean IsAcive { get; set; }

        public static async Task<UserAccount> CreateAsync(String login, String email, String password, UserAccountType accountType)
        {
            String passwordSalt = PasswordHelper.GeneratePasswordSalt(UserAccount.MaxPasswordLength);
            String hashPassword = PasswordHelper.ComputePasswordHash(password, passwordSalt);

            using (IUserAccountRepository userAccountRepository = new UserAccountRepository())
            {
                DataModels.UserAccount dataUserAccount = await userAccountRepository.CreateAsync(login, email, hashPassword, passwordSalt, (byte)accountType);
                UserAccount userAccount = UserAccount.ConvertToBusinessObject(dataUserAccount);

                return userAccount;
            }
        }

        public static async Task<UserAccount> GetUserAsync(String emailOrLogin)
        {
            if (String.IsNullOrEmpty(emailOrLogin) || String.IsNullOrEmpty(emailOrLogin.Trim()))
            {
                throw new ArgumentException("Email or login can't be null or empty.");
            }

            using (IUserAccountRepository userAccountRepository = new UserAccountRepository())
            {
                DataModels.UserAccount dataUserAccount = await userAccountRepository.GetAsync(emailOrLogin);
                UserAccount userAccount = dataUserAccount != null
                    ? ConvertToBusinessObject(dataUserAccount)
                    : null;

                return userAccount;
            }
        }

        public static async Task<UserAccount> GetUserAsync(Int32 userId)
        {
            using (IUserAccountRepository userAccountRepository = new UserAccountRepository())
            {
                DataModels.UserAccount dataUserAccount = await userAccountRepository.GetAsync(userId);
                UserAccount userAccount = dataUserAccount != null
                    ? ConvertToBusinessObject(dataUserAccount)
                    : null;

                return userAccount;
            }
        }

        public Boolean ValidatePassword(String password)
        {
            if (String.IsNullOrEmpty(password))
            {
                return false;
            }

            String hashPassword = PasswordHelper.ComputePasswordHash(password, this.PasswordSalt);

            return this.Password.Equals(hashPassword);
        }

        public async Task<Dictionary<PermissionType, AccessType>> GetAccountPermissions()
        {
            using (IPermissionsRepository permissionsRepository = new PermissionsRepository())
            {
                List<DataModels.AccountPermission> accountPermissionsDataModel =
                    (await permissionsRepository.GetAccountPermissionsAsync((byte)this.AccountType)).ToList();

                Dictionary<PermissionType, AccessType> accountPermissions =
                    accountPermissionsDataModel.ToDictionary(key => (PermissionType) key.PermissionType,
                        value => (AccessType) value.AccessType);

                return accountPermissions;
            }
        }

        public async Task SaveAsync()
        {
            using (IUserAccountRepository userAccountRepository = new UserAccountRepository())
            {
                await userAccountRepository.UpdateAsync(this.UserId, this.UserGuid, this.Login, this.Email,
                    this.Password, this.PasswordSalt, this.CreatedDateUtc, this.LastLoginDateUtc,
                    this.RemovedDateUtc, (byte) this.AccountType, this.IsAcive);
            }
        }

        public async Task DeleteAsync()
        {
            using (IUserAccountRepository userAccountRepository = new UserAccountRepository())
            {
                await userAccountRepository.DeleteAsync(this.UserId);
            }
        }

        #region Helpers

        private static UserAccount ConvertToBusinessObject(DataModels.UserAccount dataUserAccount)
        {
            Mapper.CreateMap<DataModels.UserAccount, UserAccount>()
                .ForMember(dest => dest.AccountType, opt => opt.MapFrom(src => (UserAccountType)src.AccountType))
                .ForMember(dest => dest.IsAcive, opt => opt.MapFrom(src => src.IsActive));

            UserAccount userAccount = Mapper.Map<DataModels.UserAccount, UserAccount>(dataUserAccount);

            return userAccount;
        }

        #endregion
    }
}