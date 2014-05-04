using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Indigo.WinClient.Model.Notifications;
using Indigo.WinClient.View;

namespace Indigo.WinClient.ViewModel.Partial
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class UnauthorizedViewModel : CommonPartialViewModel
    {
        /// <summary>
        /// Initializes a new instance of the UnauthorizedViewModel class.
        /// </summary>
        public UnauthorizedViewModel()
        {
        }

        #region Commands

        public ICommand NavigateToSignInPageCommand
        {
            get { return new RelayCommand(NavigateToSignInPage); }
        }

        private void NavigateToSignInPage()
        {
            base.SendNavigationMessage(ApplicationView.SignIn, NavigationToken.MainViewNavigationToken);
        }

        #endregion
    }
}