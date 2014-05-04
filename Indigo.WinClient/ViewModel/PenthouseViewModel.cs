using System.Collections.ObjectModel;
using System.Drawing;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Messaging;
using Indigo.WinClient.Model.Notifications;

namespace Indigo.WinClient.ViewModel
{
    using Microsoft.Practices.ServiceLocation;
    using System;
    using System.Linq;
    using System.Windows.Input;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;

    using Indigo.BusinessLogicLayer.Account;
    using Indigo.WinClient.Model;
    using Indigo.WinClient.View;
    using Indigo.WinClient.ViewModel.Partial;

    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class PenthouseViewModel : CommonViewModel
    {
        public delegate void LoadViewModelHandler(UserAccount user);

        public event LoadViewModelHandler LoadViewModel;

        #region Properties

        /// <summary>
        /// The <see cref="Title" /> property's name.
        /// </summary>
        public const string TitlePropertyName = "Title";

        private String _title = "Персональный кабинет";

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
                base.RaisePropertyChanged(TitlePropertyName);
            }
        }

        /// <summary>
        /// The <see cref="Actions" /> property's name.
        /// </summary>
        public const string ActionsPropertyName = "Actions";

        private ActionList _actions;

        /// <summary>
        /// Sets and gets the Actions property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ActionList Actions
        {
            get { return this._actions; }

            set
            {
                if (this._actions == value)
                {
                    return;
                }

                this._actions = value;
                base.RaisePropertyChanged(ActionsPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="SelectedActionItem" /> property's name.
        /// </summary>
        public const string SelectedActionItemPropertyName = "SelectedActionItem";

        private ActionList.Item _selectedActionItem;

        /// <summary>
        /// Sets and gets the SelectedActionItem property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ActionList.Item SelectedActionItem
        {
            get { return this._selectedActionItem; }

            set
            {
                if (this._selectedActionItem == value)
                {
                    return;
                }

                this._selectedActionItem = value;
                base.RaisePropertyChanged(SelectedActionItemPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="SelectedViewModel" /> property's name.
        /// </summary>
        public const string SelectedViewModelPropertyName = "SelectedViewModel";

        private ViewModelBase _selectedViewModel;

        /// <summary>
        /// Sets and gets the SelectedViewModel property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ViewModelBase SelectedViewModel
        {
            get
            {
                return this._selectedViewModel;
            }

            set
            {
                if (this._selectedViewModel == value)
                {
                    return;
                }

                this._selectedViewModel = value;
                base.RaisePropertyChanged(SelectedViewModelPropertyName);
            }
        }

        #endregion

        #region Commands

        public ICommand InitializeModelCommand
        {
            get
            {
                return new RelayCommand(InitializeModelAsync);
            }
        }

        private void InitializeModelAsync()
        {
            switch (this.SelectedActionItem.Permission)
            {
                case PermissionType.ReferenceInformation:
                {
                    this.SelectedViewModel = ServiceLocator.Current.GetInstance<ReferencesViewModel>();
                } break;

                case PermissionType.DocumentsCollection:
                {
                    this.SelectedViewModel = ServiceLocator.Current.GetInstance<DocumentsViewModel>();
                } break;

                case PermissionType.ProfileInformation:
                {
                    this.SelectedViewModel = ServiceLocator.Current.GetInstance<ProfileViewModel>();
                } break;

                case PermissionType.UserDatabase:
                {
                    this.SelectedViewModel = ServiceLocator.Current.GetInstance<UsersViewModel>();
                } break;

                case PermissionType.Reports:
                {
                    this.SelectedViewModel = ServiceLocator.Current.GetInstance<ReportsViewModel>();
                } break;
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the PenthouseViewModel class.
        /// </summary>
        public PenthouseViewModel()
        {
            this.LoadViewModel += async user =>
            {
                this.Actions = await ActionList.GetAccountActionList(IndigoUserPrincipal.Current.Identity.User);
                this.SelectedActionItem =
                    this.Actions.FirstOrDefault(x => x.Permission == PermissionType.ProfileInformation);
                this.SelectedViewModel = ServiceLocator.Current.GetInstance<ProfileViewModel>();
            };

            this.LoadViewModel(IndigoUserPrincipal.Current.Identity.User);

            Messenger.Default.Register<NavigationMessage>(this, NavigationToken.AddDocumentsToken, message =>
            {
                this.SelectedViewModel = ServiceLocator.Current.GetInstance<AddDocumentsViewModel>();
            });
        }

        #endregion

        #region Helpers

        private ViewModelBase GetTabContentViewModel(PermissionType permission)
        {
            switch (permission)
            {
                case PermissionType.ProfileInformation:
                {
                    return SimpleIoc.Default.GetInstance<ProfileViewModel>();
                }

                case PermissionType.DocumentsCollection:
                {
                    return SimpleIoc.Default.GetInstance<DocumentsViewModel>();
                }

                case PermissionType.ReferenceInformation:
                {
                    return SimpleIoc.Default.GetInstance<ReferencesViewModel>();
                }

                case PermissionType.UserDatabase:
                {
                    return SimpleIoc.Default.GetInstance<UsersViewModel>();
                }

                case PermissionType.Reports:
                {
                    return SimpleIoc.Default.GetInstance<ReportsViewModel>();
                }
                
                default:
                {
                    return null;
                }
            }
        }

        #endregion
    }
}