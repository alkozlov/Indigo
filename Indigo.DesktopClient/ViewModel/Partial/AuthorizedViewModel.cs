using System.Collections.Generic;
using System.Linq;
using GalaSoft.MvvmLight;
using Microsoft.Practices.ServiceLocation;

namespace Indigo.DesktopClient.ViewModel.Partial
{
    using System;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;

    using GalaSoft.MvvmLight.Command;

    using Indigo.BusinessLogicLayer.Account;
    using Indigo.DesktopClient.CommandDelegates;
    using Indigo.DesktopClient.Model.Notifications;
    using Indigo.DesktopClient.View;

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
            base.SendNavigationMessage(ApplicationView.Penthouse, NavigationToken.MainViewNavigationToken);
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
            if (!IndigoUserPrincipal.Current.Identity.IsAuthenticated)
            {
                MessageBox.Show("Не удалось распознать пользователя!");
            }
            else
            {
                await IndigoUserPrincipal.Current.SignoutAsync();
                ViewModelLocator.Cleanup();

                base.SignoutMessageSend();
                base.SendNavigationMessage(ApplicationView.HomaPage, NavigationToken.MainViewNavigationToken);
            }
        }

        public ICommand NavigateToHomePageCommand
        {
            get
            {
                return new RelayCommand(NavigateToHomePage);
            }
        }

        private void NavigateToHomePage()
        {
            base.SendNavigationMessage(ApplicationView.HomaPage, NavigationToken.MainViewNavigationToken);
        }

        #endregion
    }
}