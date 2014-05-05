namespace Indigo.BusinessLogicLayer.Account
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Threading.Tasks;

    using AutoMapper;

    using DataModels = Indigo.DataAccessLayer.Models;
    using Indigo.DataAccessLayer.IRepositories;
    using Indigo.DataAccessLayer.Repositories;

    public class UserAccountList : ReadOnlyCollection<UserAccountList.UserAccountItem>
    {
         public class UserAccountItem
         {
             public Int32 UserId { get; set; }
             public Guid UserGuid { get; set; }
             public String Email { get; set; }
             public String Login { get; set; }
             public DateTime CreatedDateUtc { get; set; }
             public UserAccountType AccountType { get; set; }
         }

        public UserAccountList(IList<UserAccountItem> list) : base(list)
        {
        }

        public static async Task<UserAccountList> GetUsersAsync()
        {
            using (IUserAccountRepository userAccountRepository = new UserAccountRepository())
            {
                var dataUserAccounts = (await userAccountRepository.GetAllUsersAsync()).ToList();
                List<UserAccountItem> userAccountItems = new List<UserAccountItem>();
                if (dataUserAccounts.Count > 0)
                {
                    userAccountItems.AddRange(dataUserAccounts.Select(ConvertToListItemObject));
                }

                UserAccountList userAccountList = new UserAccountList(userAccountItems);
                return userAccountList;
            }
        }

        private static UserAccountItem ConvertToListItemObject(DataModels.UserAccount dataUserAccount)
        {
            Mapper.CreateMap<DataModels.UserAccount, UserAccountItem>()
                .ForMember(dest => dest.AccountType, opt => opt.MapFrom(src => (UserAccountType)src.AccountType));

            UserAccountItem userAccountItem = Mapper.Map<DataModels.UserAccount, UserAccountItem>(dataUserAccount);
            return userAccountItem;
        }
    }
}