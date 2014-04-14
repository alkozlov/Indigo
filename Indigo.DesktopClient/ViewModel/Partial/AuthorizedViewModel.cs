using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Indigo.BusinessLogicLayer.Account;
using Indigo.DesktopClient.CommandDelegates;
using Indigo.DesktopClient.Model.Notifications;
using Indigo.DesktopClient.View;

namespace Indigo.DesktopClient.ViewModel.Partial
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class AuthorizedViewModel : CommonPartialViewModel
    {
        /// <summary>
        /// Initializes a new instance of the AuthorizedViewModel class.
        /// </summary>
        public AuthorizedViewModel()
        {
        }

        public override ApplicationView ViewType
        {
            get { return ApplicationView.AuthorizedCommandPanel; }
        }

        /// <summary>
        /// Gets the UserFullName.
        /// </summary>
        public String UserFullName
        {
            get
            {
                String fullName = String.Empty;
                if (IndigoUserPrincipal.Current == null || IndigoUserPrincipal.Current.Identity == null)
                {
                    MessageBox.Show("Не удалось распознать пользователя!");
                }
                else
                {
                    fullName = IndigoUserPrincipal.Current.Identity.Name;
                }

                return fullName;
            }
        }

        #region Commands

        public ICommand NavigateToPenthouseCommand
        {
            get
            {
                return new RelayCommand(NavigateToPenthouse);
            }
        }

        private void NavigateToPenthouse()
        {
            base.NavigateAction(this.ViewType, ApplicationView.Penthouse, NotificationTokens.MainViewNavigationToken);
        }

        public ICommand SignOutCommand
        {
            get
            {
                return new AsyncCommand(Signout);
            }
        }

        private async Task Signout(object o)
        {
            if (IndigoUserPrincipal.Current == null || IndigoUserPrincipal.Current.Identity == null)
            {
                MessageBox.Show("Не удалось распознать пользователя!");
            }
            else
            {
                await IndigoUserPrincipal.Current.SignoutAsync();
                base.SignoutMessageSend();
                base.NavigateAction(this.ViewType, ApplicationView.Analysis, NotificationTokens.MainViewNavigationToken);
            }
        }

        #endregion
    }
}