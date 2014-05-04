using Indigo.DesktopClient.ViewModel.Partial;

namespace Indigo.DesktopClient.ViewModel
{
    using System;
    using System.Windows.Input;
    using Microsoft.Practices.ServiceLocation;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Messaging;
    using Indigo.DesktopClient.Model;
    using Indigo.DesktopClient.Model.Notifications;
    using Indigo.DesktopClient.View;

    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : CommonViewModel
    {
        private readonly IDataService _dataService;

        #region Properties

        /// <summary>
        /// The <see cref="Title" /> property's name.
        /// </summary>
        public const string TitlePropertyName = "Базовое окно программы";

        private string _title = string.Empty;

        /// <summary>
        /// Gets the Title property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public string Title
        {
            get
            {
                return _title;
            }

            set
            {
                if (_title == value)
                {
                    return;
                }

                _title = value;
                RaisePropertyChanged(TitlePropertyName);
            }
        }

        /// <summary>
        /// The <see cref="CurrentViewModel" /> property's name.
        /// </summary>
        public const string CurrentViewModelPropertyName = "CurrentViewModel";

        private ViewModelBase _currentViewModel;

        /// <summary>
        /// The <see cref="IsJumpBackButtonAvailable" /> property's name.
        /// </summary>
        public const String IsJumpBackButtonAvailablePropertyName = "IsJumpBackButtonAvailable";

        private Boolean _isJumpBackButtonAvailable;

        /// <summary>
        /// Sets and gets the IsJumpBackButtonAvailable property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Boolean IsJumpBackButtonAvailable
        {
            get
            {
                return this._isJumpBackButtonAvailable;
            }

            set
            {
                if (this._isJumpBackButtonAvailable == value)
                {
                    return;
                }

                this._isJumpBackButtonAvailable = value;
                RaisePropertyChanged(IsJumpBackButtonAvailablePropertyName);
            }
        }

        /// <summary>
        /// Sets and gets the CurrentViewModel property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ViewModelBase CurrentViewModel
        {
            get
            {
                return _currentViewModel;
            }

            set
            {
                if (_currentViewModel == value)
                {
                    return;
                }

                this._currentViewModel = value;
                RaisePropertyChanged(CurrentViewModelPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="CommandPanelViewModel" /> property's name.
        /// </summary>
        public const string CommandPanelViewModelPropertyName = "CommandPanelViewModel";

        private ViewModelBase _commandPanelViewModel;

        /// <summary>
        /// Sets and gets the CommandPanelViewModel property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ViewModelBase CommandPanelViewModel
        {
            get
            {
                return this._commandPanelViewModel;
            }

            set
            {
                if (this._commandPanelViewModel == value)
                {
                    return;
                }

                this._commandPanelViewModel = value;
                base.RaisePropertyChanged(CommandPanelViewModelPropertyName);
            }
        }

        #endregion

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

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel(IDataService dataService)
        {
            _dataService = dataService;
            _dataService.GetData(
                (item, error) =>
                {
                    if (error != null)
                    {
                        // Report error here
                        return;
                    }

                    Title = item.Title;
                });

            // Set AnalysisView as start page
            this.CurrentViewModel = ServiceLocator.Current.GetInstance<HomePageViewModel>();

            this.CommandPanelViewModel = ServiceLocator.Current.GetInstance<UnauthorizedViewModel>();

            #region Notifications

            #region Navigation messages

            Messenger.Default.Register<NavigationMessage>(this, NavigationToken.MainViewNavigationToken, message =>
            {
                if (message != null)
                {
                    ViewModelBase targetViewModel = null;;

                    switch (message.TargetView)
                    {
                        case ApplicationView.Analysis:
                        {
                            targetViewModel = ServiceLocator.Current.GetInstance<AnalysisViewModel>();
                        } break;

                        case ApplicationView.SignIn:
                        {
                            targetViewModel = ServiceLocator.Current.GetInstance<SignInViewModel>();
                        } break;

                        case ApplicationView.SignUp:
                        {

                        } break;

                        case ApplicationView.PasswordRecovery:
                        {

                        } break;

                        case ApplicationView.Penthouse:
                        {
                            targetViewModel = ServiceLocator.Current.GetInstance<PenthouseViewModel>();
                        } break;

                        case ApplicationView.HomaPage:
                        {
                            targetViewModel = ServiceLocator.Current.GetInstance<HomePageViewModel>();
                        } break;

                        case ApplicationView.DocumentAnalysis:
                        {
                            targetViewModel = ServiceLocator.Current.GetInstance<DocumentAnalysisViewModel>();
                        } break;

                        case ApplicationView.TextAnalisys:
                        {
                            targetViewModel = ServiceLocator.Current.GetInstance<TextAnalysisViewModel>();
                        } break;
                    }

                    if (targetViewModel != null)
                    {
                        this.CurrentViewModel = targetViewModel;
                    }
                }
            });

            #endregion

            #region Autorization panel

            Messenger.Default.Register<AuthorizationMessage>(this, NavigationToken.AuthorizationPanelToken, message =>
            {
                if (message.IsAuthorized)
                {
                    this.CommandPanelViewModel = ServiceLocator.Current.GetInstance<AuthorizedViewModel>();
                }
                else
                {
                    this.CommandPanelViewModel = ServiceLocator.Current.GetInstance<UnauthorizedViewModel>();
                }
            });

            #endregion

            #endregion
        }

        #endregion

        #region Helpers

        public override void Cleanup()
        {
            // Clean up if needed
            this.CurrentViewModel = null;

            base.Cleanup();
        }

        #endregion
    }
}