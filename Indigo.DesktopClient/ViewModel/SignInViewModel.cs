using Indigo.DesktopClient.Model.Notifications;

namespace Indigo.DesktopClient.ViewModel
{
    using System;
    using System.ComponentModel;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;

    using Indigo.BusinessLogicLayer.Account;
    using Indigo.DesktopClient.CommandDelegates;
    using Indigo.DesktopClient.View;

    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class SignInViewModel : CommonViewModel
    {
        #region Override

        public override ApplicationView ViewType
        {
            get { return ApplicationView.SignIn; }
        }

        #endregion

        #region Properties

        /// <summary>
        /// The <see cref="Title" /> property's name.
        /// </summary>
        public const string TitlePropertyName = "Title";

        private String _title = "Вход в личный кабинет";

        /// <summary>
        /// Sets and gets the Title property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public String Title
        {
            get { return this._title; }

            set
            {
                if (this._title == value)
                {
                    return;
                }

                this._title = value;
                RaisePropertyChanged(TitlePropertyName);
            }
        }

        /// <summary>
        /// The <see cref="EmailOrLogin" /> property's name.
        /// </summary>
        public const string EmailOrLoginPropertyName = "EmailOrLogin";

        private String _emailOrLogin;

        /// <summary>
        /// Sets and gets the EmailOrLogin property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public String EmailOrLogin
        {
            get
            {
                return _emailOrLogin;
            }

            set
            {
                if (_emailOrLogin == value)
                {
                    return;
                }

                this._emailOrLogin = value;
                base.RaisePropertyChanged(EmailOrLoginPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="Password" /> property's name.
        /// </summary>
        public const string PasswordPropertyName = "Password";

        private String _password;

        /// <summary>
        /// Sets and gets the Password property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public String Password
        {
            get
            {
                return _password;
            }

            set
            {
                if (_password == value)
                {
                    return;
                }

                this._password = value;
                base.RaisePropertyChanged(PasswordPropertyName);
            }
        }

        #endregion

        #region Commands

        public ICommand SignInCommand
        {
            get
            {
                return new AsyncDelegateCommand(SignIn);
            }
        }

        private async Task SignIn(object o)
        {
            if (String.IsNullOrEmpty(this.EmailOrLogin) || String.IsNullOrEmpty(this.Password))
            {
                MessageBox.Show("Введите логин и пароль!");
                return;
            }

            try
            {
                IndigoUserPrincipal principal = await IndigoUserPrincipal.LoginAsync(this.EmailOrLogin, this.Password);
                principal.AssignPrincipalToCurrentContext();

                base.NavigateAction(this.ViewType, ApplicationView.Penthouse, NotificationTokens.MainViewNavigationToken);
            }
            catch (LoginException e)
            {
                MessageBox.Show(e.Reason.ToString());
            }
            catch (Exception e)
            {
                MessageBox.Show("Неизвестное исключение!");
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the SignInViewModel class.
        /// </summary>
        public SignInViewModel()
        {
            
        }

        #endregion
    }
}