namespace Indigo.DesktopClient.ViewModel.Partial
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Data;
    using System.Globalization;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using Infragistics.Controls.Grids;

    using Indigo.BusinessLogicLayer.Document;
    using Indigo.DesktopClient.CommandDelegates;
    using Indigo.DesktopClient.Model.Notifications;
    using Indigo.DesktopClient.Model.PenthouseModels;

    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class SubjectsViewModel : CommonPartialViewModel
    {
        private const String SubjectFieldKey = "SubjectId";
        private const Int32 MaxSubjectLength = 100;

        private event EventHandler InitializeView;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the SubjectsViewModel class.
        /// </summary>
        public SubjectsViewModel()
        {
            // Default initialization
            this.Subjects = new ObservableCollection<SubjectModel>();
            this.NewSubject = new NewSubjectModel();
            this.NewSubjectNotification = new SystemNotification();
            this.ToolbarNotification = new SystemNotification();

            this.InitializeView += async (sender, args) =>
            {
                await LoadSubjectsAsync();
            };

            this.InitializeView(this, null);
        }

        #endregion

        #region Properties

        /// <summary>
        /// The <see cref="Subjects" /> property's name.
        /// </summary>
        public const string SubjectsPropertyName = "Subjects";

        private ObservableCollection<SubjectModel> _subjects;

        /// <summary>
        /// Sets and gets the Subjects property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<SubjectModel> Subjects
        {
            get
            {
                return this._subjects;
            }

            set
            {
                if (this._subjects == value)
                {
                    return;
                }

                this._subjects = value;
                base.RaisePropertyChanged(SubjectsPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="NewSubject" /> property's name.
        /// </summary>
        public const string NewSubjectPropertyName = "NewSubject";

        private NewSubjectModel _newSubject = new NewSubjectModel();

        /// <summary>
        /// Sets and gets the NewSubjectSubject property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public NewSubjectModel NewSubject
        {
            get
            {
                return this._newSubject;
            }

            set
            {
                if (this._newSubject == value)
                {
                    return;
                }

                this._newSubject = value;
                base.RaisePropertyChanged(NewSubjectPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="NewSubjectNotification" /> property's name.
        /// </summary>
        public const string NewSubjectNotificationPropertyName = "NewSubjectNotification";

        private SystemNotification _newSubjectNotification;

        /// <summary>
        /// Sets and gets the NewSubjectNotification property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public SystemNotification NewSubjectNotification
        {
            get
            {
                return this._newSubjectNotification;
            }

            set
            {
                if (this._newSubjectNotification == value)
                {
                    return;
                }

                this._newSubjectNotification = value;
                base.RaisePropertyChanged(NewSubjectNotificationPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="ToolbarNotification" /> property's name.
        /// </summary>
        public const string ToolbarNotificationPropertyName = "ToolbarNotification";

        private SystemNotification _toolbarNotification;

        /// <summary>
        /// Sets and gets the ToolbarNotification property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public SystemNotification ToolbarNotification
        {
            get
            {
                return this._toolbarNotification;
            }

            set
            {
                if (this._toolbarNotification == value)
                {
                    return;
                }

                this._toolbarNotification = value;
                base.RaisePropertyChanged(ToolbarNotificationPropertyName);
            }
        }

        #endregion

        #region Commands

        public ICommand RemoveSelectedSubjectsCommand
        {
            get
            {
                return new AsyncCommand(RemoveSelectedSubjectsAsync);
            }
        }

        private async Task RemoveSelectedSubjectsAsync(object parameter)
        {
            SelectedRowsCollection selectedRows = parameter as SelectedRowsCollection;

            if (selectedRows != null && selectedRows.Count > 0)
            {
                try
                {
                    Int32 removedRecords = 0;
                    foreach (var selectedRow in selectedRows)
                    {
                        var subjectKeyColumn = selectedRow.Columns[SubjectFieldKey];
                        if (subjectKeyColumn != null)
                        {
                            Int32 subjectId = Int32.Parse(selectedRow.Cells[subjectKeyColumn.Key].Value.ToString());
                            Subject subject = await Subject.GetByIdAsync(subjectId);
                            if (subject != null)
                            {
                                // Remove subject from database
                                await subject.DeleteAsync();
                                removedRecords++;
                            }
                        }
                    }

                    if (removedRecords > 0)
                    {
                        String toolbarStatusMessage;
                        if (removedRecords.ToString(CultureInfo.InvariantCulture).EndsWith("1"))
                        {
                            toolbarStatusMessage = String.Format("Удалена {0} запись.", removedRecords);
                        }
                        else
                        {
                            if (removedRecords.ToString(CultureInfo.InvariantCulture).EndsWith("2") ||
                                removedRecords.ToString(CultureInfo.InvariantCulture).EndsWith("3") ||
                                removedRecords.ToString(CultureInfo.InvariantCulture).EndsWith("4"))
                            {
                                toolbarStatusMessage = String.Format("Удалено {0} записи.", removedRecords);
                            }
                            else
                            {
                                toolbarStatusMessage = String.Format("Удалено {0} записей.", removedRecords);
                            }
                        }
                        
                        this.ToolbarNotification = base.GetSuccessNotification(toolbarStatusMessage);
                    }
                    await LoadSubjectsAsync();
                }
                catch (Exception e)
                {
                    String exceptionMessage = "Произошло непредвиденное исключение. Попробуйте еще раз.";
                    this.ToolbarNotification = base.GetErrorNotification(exceptionMessage);
                }
            }
        }

        public ICommand AddNewSubjectCommand
        {
            get { return new AsyncCommand(AddNewSubjectAsync); }
        }

        private async Task AddNewSubjectAsync(object parameter)
        {
            // Validation
            SystemNotification validateNotification;
            if (!IsNewSubjectModelValid(out validateNotification))
            {
                this.NewSubjectNotification = validateNotification;
                return;
            }

            try
            {
                Subject newSubject = await Subject.CreateAsync(this.NewSubject.SubjectName);
                if (newSubject != null)
                {
                    await LoadSubjectsAsync();
                    String statusMessage = String.Format("Новая тематика '{0}' успешно добавлена.",
                        newSubject.SubjectHeader);
                    this.NewSubjectNotification = base.GetSuccessNotification(statusMessage);
                }
                else
                {
                    this.NewSubjectNotification =
                        base.GetErrorNotification("Что-то пошло не так. Не удалось добавить тематику.");
                }
            }
            catch (DuplicateNameException e)
            {
                String exceptionMessage = String.Format("Тематика '{0}' уже есть в базе.", this.NewSubject.SubjectName);
                this.NewSubjectNotification = base.GetErrorNotification(exceptionMessage);
            }
            catch (ArgumentNullException e)
            {
                String exceptionMessage = "Тематика не может быть пустой.";
                this.NewSubjectNotification = base.GetErrorNotification(exceptionMessage);
            }
            catch (Exception e)
            {
                String exceptionMessage = "Произошло непредвиденное исключение. Попробуйте еще раз.";
                this.NewSubjectNotification = base.GetErrorNotification(exceptionMessage);
            }
        }

        #endregion

        #region Helpers

        private async Task LoadSubjectsAsync()
        {
            SubjectList subjectsList = await SubjectList.GetSubjectsAsync();
            List<SubjectModel> subjectModels = subjectsList.Select(item => new SubjectModel
            {
                SubjectId = item.SubjectId,
                SubjectHeader = item.SubjectHeader
            }).ToList();
            this.Subjects = new ObservableCollection<SubjectModel>(subjectModels);
        }

        private Boolean IsNewSubjectModelValid(out SystemNotification systemNotification)
        {
            systemNotification = null;
            if (String.IsNullOrEmpty(this.NewSubject.SubjectName) || String.IsNullOrEmpty(this.NewSubject.SubjectName.Trim()))
            {
                systemNotification = base.GetErrorNotification("Тематика не может быть пустой.");
                return false;
            }

            if (this.NewSubject.SubjectName.Length > MaxSubjectLength)
            {
                String warningMessage = String.Format("Название тематики не должно превышать {0} символов.", MaxSubjectLength);
                systemNotification = base.GetWarningNotification(warningMessage);
                return false;
            }

            return true;
        }

        #endregion
    }
}