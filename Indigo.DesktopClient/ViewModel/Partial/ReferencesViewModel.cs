namespace Indigo.DesktopClient.ViewModel.Partial
{
    using System.Collections.ObjectModel;
    using System.Linq;
    using System.Windows.Input;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Command;
    using GalaSoft.MvvmLight.Ioc;
    using GalaSoft.MvvmLight.Messaging;

    using Indigo.DesktopClient.Model.Notifications;
    using Indigo.DesktopClient.Model.PenthouseModels;
    using Indigo.DesktopClient.View;

    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class ReferencesViewModel : CommonPartialViewModel
    {
        private const ApplicationView DefaultApplicationView = ApplicationView.Subjects;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the ReferencesViewModel class.
        /// </summary>
        public ReferencesViewModel()
        {
            this.References = new ObservableCollection<ReferenceItem>
            {
                new ReferenceItem
                {
                    Header = "Категории",
                    Type = ReferenceType.Subjects
                },
                 new ReferenceItem
                {
                    Header = "Стоп-слова",
                    Type = ReferenceType.StopWords
                }
            };

            this.SelectedReference = this.References.First();
            this.SelectedEntity = SimpleIoc.Default.GetInstance<SubjectsViewModel>();

            #region Messages

            Messenger.Default.Register<NavigationMessage>(this, NavigationToken.ReferencesToken, message =>
            {
                switch (message.TargetView)
                {
                    case ApplicationView.Subjects:
                    {
                        this.SelectedEntity = SimpleIoc.Default.GetInstance<SubjectsViewModel>();
                    } break;

                    case ApplicationView.StopWords:
                    {
                        this.SelectedEntity = SimpleIoc.Default.GetInstance<StopWordsViewModel>();
                    } break;

                    default:
                    {
                        this.SelectedEntity = SimpleIoc.Default.GetInstance<SubjectsViewModel>();
                    } break;
                }
            });

            #endregion
        }

        #endregion

        #region Properties

        /// <summary>
        /// The <see cref="References" /> property's name.
        /// </summary>
        public const string ReferencesPropertyName = "References";

        private ObservableCollection<ReferenceItem> _references;

        /// <summary>
        /// Sets and gets the References property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<ReferenceItem> References
        {
            get
            {
                return this._references;
            }

            set
            {
                if (this._references == value)
                {
                    return;
                }

                this._references = value;
                base.RaisePropertyChanged(ReferencesPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="SelectedReference" /> property's name.
        /// </summary>
        public const string SelectedReferencePropertyName = "SelectedReference";

        private ReferenceItem _selectedReference;

        /// <summary>
        /// Sets and gets the SelectedReference property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ReferenceItem SelectedReference
        {
            get
            {
                return this._selectedReference;
            }

            set
            {
                if (this._selectedReference == value)
                {
                    return;
                }

                this._selectedReference = value;
                base.RaisePropertyChanged(SelectedReferencePropertyName);
            }
        }

        /// <summary>
        /// The <see cref="SelectedEntity" /> property's name.
        /// </summary>
        public const string SelectedEntityPropertyName = "SelectedEntity";

        private ViewModelBase _selectedEntity;

        /// <summary>
        /// Sets and gets the SelectedEntity property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ViewModelBase SelectedEntity
        {
            get
            {
                return this._selectedEntity;
            }

            set
            {
                if (this._selectedEntity == value)
                {
                    return;
                }

                this._selectedEntity = value;
                base.RaisePropertyChanged(SelectedEntityPropertyName);
            }
        }

        #endregion

        #region Commands

        public ICommand ChangeReferenceViewCommand
        {
            get
            {
                return new RelayCommand<object>(ChangeReferenceView);
            }
        }

        private void ChangeReferenceView(object o)
        {
            ReferenceItem selectedReferenceItem = o as ReferenceItem;
            if (selectedReferenceItem != null)
            {
                ApplicationView selectedApplicationView;
                switch (selectedReferenceItem.Type)
                {
                    case ReferenceType.Subjects:
                    {
                        selectedApplicationView = ApplicationView.Subjects;
                    } break;

                    case ReferenceType.StopWords:
                    {
                        selectedApplicationView = ApplicationView.StopWords;
                    } break;

                    default:
                    {
                        selectedApplicationView = DefaultApplicationView;
                    } break;
                }

                base.SendNavigationMessage(selectedApplicationView, NavigationToken.ReferencesToken);
            }
        }

        #endregion
    }
}