using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
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
    public class UnauthorizedViewModel : CommonPartialViewModel
    {
        /// <summary>
        /// Initializes a new instance of the UnauthorizedViewModel class.
        /// </summary>
        public UnauthorizedViewModel()
        {
        }

        public override ApplicationView ViewType
        {
            get { return ApplicationView.UnauthorizedCommandPanel; }
        }

        #region Commands

        public ICommand NavigateToSignInPageCommand
        {
            get { return new RelayCommand(NavigateToSignInPage); }
        }

        private void NavigateToSignInPage()
        {
            base.NavigateAction(this.ViewType, ApplicationView.SignIn, NotificationTokens.MainViewNavigationToken);
        }

        #endregion
    }
}