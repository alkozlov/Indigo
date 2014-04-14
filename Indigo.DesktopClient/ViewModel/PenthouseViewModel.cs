using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Indigo.BusinessLogicLayer.Account;
using Indigo.DesktopClient.Model;

namespace Indigo.DesktopClient.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class PenthouseViewModel : ViewModelBase
    {
        public delegate void LoadViewModelHandler(UserAccount user);

        public event LoadViewModelHandler LoadViewModel;

        /// <summary>
        /// Initializes a new instance of the PenthouseViewModel class.
        /// </summary>
        public PenthouseViewModel()
        {
            this.LoadViewModel += async user =>
            {
                this.Actions = await ActionList.GetAccountActionList(IndigoUserPrincipal.Current.Identity.User);
                this.SelectedActionItem = this.Actions.FirstOrDefault(x => x.Permission == PermissionType.ProfileInformation);
            };

            this.LoadViewModel(IndigoUserPrincipal.Current.Identity.User);
        }

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
            get
            {
                return this._title;
            }

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
            get
            {
                return this._actions;
            }

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
            get
            {
                return this._selectedActionItem;
            }

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
            Debug.WriteLine(String.Format("New view is {0}", this.SelectedActionItem.ActionName));
        }

        #endregion
    }
}