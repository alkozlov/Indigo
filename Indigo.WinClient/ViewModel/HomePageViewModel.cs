using System;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Indigo.WinClient.Model.Notifications;
using Indigo.WinClient.View;

namespace Indigo.WinClient.ViewModel
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
            base.SendNavigationMessage(ApplicationView.DocumentAnalysis, NavigationToken.MainViewNavigationToken);
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
            base.SendNavigationMessage(ApplicationView.TextAnalisys, NavigationToken.MainViewNavigationToken);
        }

        #endregion
    }
}