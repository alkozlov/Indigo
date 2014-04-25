using System;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Indigo.DesktopClient.Model.Notifications;
using Indigo.DesktopClient.View;

namespace Indigo.DesktopClient.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class HomePageViewModel : CommonViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the HomePageViewModel class.
        /// </summary>
        public HomePageViewModel()
        {
        }

        #endregion

        #region Overrides

        public override ApplicationView ViewType
        {
            get { return ApplicationView.HomaPage; }
        }

        #endregion

        #region Commands

        public ICommand DocumntAnalisysCommand
        {
            get
            {
                return new RelayCommand(DocumentAnalysis);
            }
        }

        private void DocumentAnalysis()
        {
            base.NavigateAction(this.ViewType, ApplicationView.DocumentAnalysis, NotificationTokens.MainViewNavigationToken);
        }

        public ICommand TextAnalisysCommand
        {
            get
            {
                return new RelayCommand(TextAnalysis);
            }
        }

        private void TextAnalysis()
        {
            base.NavigateAction(this.ViewType, ApplicationView.TextAnalisys, NotificationTokens.MainViewNavigationToken);
        }

        #endregion
    }
}