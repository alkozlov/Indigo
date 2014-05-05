using Indigo.BusinessLogicLayer.Account;

namespace Indigo.DesktopClient.ViewModel.Partial
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using Infragistics.Controls.Grids;

    using Indigo.BusinessLogicLayer.Document;
    using Indigo.DesktopClient.CommandDelegates;
    using Indigo.DesktopClient.Model.Notifications;
    using Indigo.DesktopClient.Model.PenthouseModels;

    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class UsersViewModel : CommonPartialViewModel
    {
        private event EventHandler InitializeView;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the UsersViewModel class.
        /// </summary>
        public UsersViewModel()
        {
            this.InitializeView += async (sender, args) =>
            {
                await this.LoadUsersAsync();
            };

            this.InitializeView(this, null);
        }

        #endregion

        #region Properties

        /// <summary>
        /// The <see cref="Users" /> property's name.
        /// </summary>
        public const string UsersPropertyName = "Users";

        private ObservableCollection<UserModel> _users;

        /// <summary>
        /// Sets and gets the Users property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<UserModel> Users
        {
            get
            {
                return this._users;
            }

            set
            {
                if (this._users == value)
                {
                    return;
                }

                this._users = value;
                base.RaisePropertyChanged(UsersPropertyName);
            }
        }

        #endregion

        #region Helpers

        private async Task LoadUsersAsync()
        {
            UserAccountList userAccountList = await UserAccountList.GetUsersAsync();
            List<UserModel> userModels = userAccountList.Select(item => new UserModel
            {
                UserAccountId = item.UserId,
                Login = item.Login,
                Email = item.Email,
                CreatedDateUtc = item.CreatedDateUtc,
                AccountType = item.AccountType
            }).ToList();

            this.Users = new ObservableCollection<UserModel>(userModels);
        }

        #endregion
    }
}