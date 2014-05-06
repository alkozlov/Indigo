using System.Net.Mail;
using System.Windows.Controls;
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
        private const String UserFieldKey = "UserAccountId";

        private event EventHandler InitializeView;

        private readonly Dictionary<UserAccountType, String> _accountTypesDictionary = new Dictionary<UserAccountType, String>
        {
            {UserAccountType.Admin, "Администратор"},
            {UserAccountType.Lecturer, "Преподаватель"},
            {UserAccountType.Viewer, "Наблюдатель"}
        };

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the UsersViewModel class.
        /// </summary>
        public UsersViewModel()
        {
            // Initialization
            this.NewUser = new NewUserModel();
            this.AccountTypes = _accountTypesDictionary.Select(x => new AccountTypeModel
            {
                AccountType = x.Key,
                AccountTypeName = x.Value
            }).ToList();
            this.NewUser.AccountTypeModel = this.AccountTypes.First();

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

        /// <summary>
        /// The <see cref="ToolbarNotification" /> property's name.
        /// </summary>
        public const string ToolbarNotificationPropertyName = "ToolbarNotification";

        private SystemNotification _toolbarNotification;

        /// <summary>
        /// Sets and gets the ToolbarNotification property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public SystemNotification ToolbarNotification
        {
            get
            {
                return this._toolbarNotification;
            }

            set
            {
                if (this._toolbarNotification == value)
                {
                    return;
                }

                this._toolbarNotification = value;
                base.RaisePropertyChanged(ToolbarNotificationPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="NewUser" /> property's name.
        /// </summary>
        public const string NewUserPropertyName = "NewUser";

        private NewUserModel _newUser;

        /// <summary>
        /// Sets and gets the NewUser property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public NewUserModel NewUser
        {
            get
            {
                return this._newUser;
            }

            set
            {
                if (this._newUser == value)
                {
                    return;
                }

                this._newUser = value;
                base.RaisePropertyChanged(NewUserPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="NewUserNotification" /> property's name.
        /// </summary>
        public const string NewUserNotificationPropertyName = "NewUserNotification";

        private SystemNotification _newUserNotification;

        /// <summary>
        /// Sets and gets the NewUserNotification property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public SystemNotification NewUserNotification
        {
            get
            {
                return this._newUserNotification;
            }

            set
            {
                if (this._newUserNotification == value)
                {
                    return;
                }

                this._newUserNotification = value;
                base.RaisePropertyChanged(NewUserNotificationPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="AccountTypes" /> property's name.
        /// </summary>
        public const string AccountTypesPropertyName = "AccountTypes";

        private List<AccountTypeModel> _accountTypes;

        /// <summary>
        /// Sets and gets the AccountTypes property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public List<AccountTypeModel> AccountTypes
        {
            get
            {
                return this._accountTypes;
            }

            set
            {
                if (this._accountTypes == value)
                {
                    return;
                }

                this._accountTypes = value;
                base.RaisePropertyChanged(AccountTypesPropertyName);
            }
        }

        #endregion

        #region Command

        public ICommand RemoveSelectedUsersCommand
        {
            get
            {
                return new AsyncCommand(RemoveSelectedUsersAsync);
            }
        }

        private async Task RemoveSelectedUsersAsync(object parameter)
        {
            SelectedRowsCollection selectedRows = parameter as SelectedRowsCollection;

            if (selectedRows != null && selectedRows.Count > 0)
            {
                try
                {
                    Int32 removedRecords = 0;
                    foreach (var selectedRow in selectedRows)
                    {
                        var userKeyColumn = selectedRow.Columns[UserFieldKey];
                        if (userKeyColumn != null)
                        {
                            Int32 userAccountId = Int32.Parse(selectedRow.Cells[userKeyColumn.Key].Value.ToString());
                            UserAccount userAccount = await UserAccount.GetUserAsync(userAccountId);
                            if (userAccount != null)
                            {
                                // Remove subject from database
                                await userAccount.DeleteAsync();
                                removedRecords++;
                            }
                        }
                    }

                    if (removedRecords > 0)
                    {
                        String toolbarStatusMessage;
                        if (removedRecords.ToString(CultureInfo.InvariantCulture).EndsWith("1"))
                        {
                            toolbarStatusMessage = String.Format("Удалена {0} запись.", removedRecords);
                        }
                        else
                        {
                            if (removedRecords.ToString(CultureInfo.InvariantCulture).EndsWith("2") ||
                                removedRecords.ToString(CultureInfo.InvariantCulture).EndsWith("3") ||
                                removedRecords.ToString(CultureInfo.InvariantCulture).EndsWith("4"))
                            {
                                toolbarStatusMessage = String.Format("Удалено {0} записи.", removedRecords);
                            }
                            else
                            {
                                toolbarStatusMessage = String.Format("Удалено {0} записей.", removedRecords);
                            }
                        }

                        this.ToolbarNotification = base.GetSuccessNotification(toolbarStatusMessage);
                    }
                    await LoadUsersAsync();
                }
                catch (Exception e)
                {
                    String exceptionMessage = "Произошло непредвиденное исключение. Попробуйте еще раз.";
                    this.ToolbarNotification = base.GetErrorNotification(exceptionMessage);
                }
            }
        }

        public ICommand AddNewUserCommand
        {
            get
            {
                return new AsyncCommand(AddNewUserAsync);
            }
        }

        private async Task AddNewUserAsync(object o)
        {
            // Validation
            SystemNotification validationNitification = await this.ValidateNewUser();
            if (validationNitification != null)
            {
                this.NewUserNotification = validationNitification;
                return;
            }

            try
            {
                String password = this.NewUser.Login;
                UserAccount userAccount =
                    await UserAccount.CreateAsync(this.NewUser.Login, this.NewUser.Email, password,
                        this.NewUser.AccountTypeModel.AccountType);

                if (userAccount != null)
                {
                    await this.LoadUsersAsync();
                    String message = String.Format("Новый пользователь {0} ({1}) успешно добавлен в базу.",
                        this.NewUser.Login, this.NewUser.Email);
                    this.NewUserNotification = base.GetSuccessNotification(message);
                }
                else
                {
                    this.NewUserNotification =
                        base.GetErrorNotification("Что-то пошло не так. Не удалось добавить пользователя.");
                }
            }
            catch (Exception)
            {
                String exceptionMessage = "Произошло непредвиденное исключение. Попробуйте еще раз.";
                this.NewUserNotification = base.GetErrorNotification(exceptionMessage);
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
                AccountTypeModel = new AccountTypeModel
                {
                    AccountType = item.AccountType,
                    AccountTypeName = _accountTypesDictionary[item.AccountType]
                }
            }).ToList();

            this.Users = new ObservableCollection<UserModel>(userModels);
        }

        public async Task<SystemNotification> ValidateNewUser()
        {
            SystemNotification systemNotification = null;

            #region Email

            if (String.IsNullOrEmpty(this.NewUser.Email) || String.IsNullOrEmpty(this.NewUser.Email.Trim()))
            {
                systemNotification = base.GetErrorNotification("Заполните адрес электронной почты.");
                return systemNotification;
            }

            try
            {
                MailAddress mailAddress = new MailAddress(this.NewUser.Email.Trim());
            }
            catch (FormatException e)
            {
                systemNotification = base.GetErrorNotification("Некорректный адрес электронной почты.");
                return systemNotification;
            }
            catch (Exception e)
            {
                systemNotification = base.GetErrorNotification("Непредвиденное исключение при проверке электронной почты.");
                return systemNotification;
            }

            UserAccount userAccount = await UserAccount.GetUserAsync(this.NewUser.Email.Trim());
            if (userAccount != null)
            {
                systemNotification = base.GetErrorNotification("Такой адрес электронной почты уже есть в базе.");
                return systemNotification;
            }

            #endregion

            #region Login

            if (String.IsNullOrEmpty(this.NewUser.Login) || String.IsNullOrEmpty(this.NewUser.Login.Trim()))
            {
                systemNotification = base.GetErrorNotification("Введите логин нового пользователя.");
                return systemNotification;
            }

            userAccount = await UserAccount.GetUserAsync(this.NewUser.Login.Trim());
            if (userAccount != null)
            {
                systemNotification = base.GetErrorNotification("Такой логин уже есть в базе.");
                return systemNotification;
            }

            #endregion

            #region Account type

            if (this.NewUser.AccountTypeModel == null || this.NewUser.AccountTypeModel.AccountType == UserAccountType.Unknown)
            {
                systemNotification = base.GetErrorNotification("Укажите типо аккаунта нового пользователя.");
                return systemNotification;
            }

            #endregion

            return systemNotification;
        }

        #endregion
    }
}