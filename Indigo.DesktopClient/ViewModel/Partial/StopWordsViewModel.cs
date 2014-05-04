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
    public class StopWordsViewModel : CommonPartialViewModel
    {
        private const String StopWordFieldKey = "StopWordId";
        private const Int32 MaxStopWordLength = 100;

        private event EventHandler InitializeView;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the StopWordsViewModel class.
        /// </summary>
        public StopWordsViewModel()
        {
            // Default initialization
            this.StopWords = new ObservableCollection<StopWordModel>();
            this.NewStopWord = new NewStopWordModel();
            this.NewStopWordNotification = new SystemNotification();
            this.ToolbarNotification = new SystemNotification();

            this.InitializeView += async (sender, args) =>
            {
                await LoadStopWordsAsync();
            };

            this.InitializeView(this, null);
        }

        #endregion

        #region Properties

        /// <summary>
        /// The <see cref="StopWords" /> property's name.
        /// </summary>
        public const string StopWordsPropertyName = "StopWords";

        private ObservableCollection<StopWordModel> _stopWords;

        /// <summary>
        /// Sets and gets the StopWords property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<StopWordModel> StopWords
        {
            get
            {
                return this._stopWords;
            }

            set
            {
                if (this._stopWords == value)
                {
                    return;
                }

                this._stopWords = value;
                base.RaisePropertyChanged(StopWordsPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="NewStopWord" /> property's name.
        /// </summary>
        public const string NewStopWordPropertyName = "NewStopWord";

        private NewStopWordModel _newStopWordModel;

        /// <summary>
        /// Sets and gets the NewStopWord property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public NewStopWordModel NewStopWord
        {
            get
            {
                return this._newStopWordModel;
            }

            set
            {
                if (this._newStopWordModel == value)
                {
                    return;
                }

                this._newStopWordModel = value;
                base.RaisePropertyChanged(NewStopWordPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="NewStopWordNotification" /> property's name.
        /// </summary>
        public const string NewStopWordNotificationPropertyName = "NewStopWordNotification";

        private SystemNotification _newStopWordNotification;

        /// <summary>
        /// Sets and gets the NewStopWordNotification property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public SystemNotification NewStopWordNotification
        {
            get
            {
                return this._newStopWordNotification;
            }

            set
            {
                if (this._newStopWordNotification == value)
                {
                    return;
                }

                this._newStopWordNotification = value;
                base.RaisePropertyChanged(NewStopWordNotificationPropertyName);
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

        public ICommand RemoveSelectedStopWordsCommand
        {
            get
            {
                return new AsyncCommand(RemoveSelectedStopWordsAsync);
            }
        }

        private async Task RemoveSelectedStopWordsAsync(object parameter)
        {
            SelectedRowsCollection selectedRows = parameter as SelectedRowsCollection;

            if (selectedRows != null && selectedRows.Count > 0)
            {
                try
                {
                    Int32 removedRecords = 0;
                    foreach (var selectedRow in selectedRows)
                    {
                        var stopWordKeyColumn = selectedRow.Columns[StopWordFieldKey];
                        if (stopWordKeyColumn != null)
                        {
                            Int32 stopWordId = Int32.Parse(selectedRow.Cells[stopWordKeyColumn.Key].Value.ToString());
                            StopWord stopWord = await StopWord.GetAsync(stopWordId);
                            if (stopWord != null)
                            {
                                // Remove subject from database
                                await stopWord.DeleteAsync();
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
                    await LoadStopWordsAsync();
                }
                catch (Exception e)
                {
                    String exceptionMessage = "Произошло непредвиденное исключение. Попробуйте еще раз.";
                    this.ToolbarNotification = base.GetErrorNotification(exceptionMessage);
                }
            }
        }

        public ICommand AddNewStopWordCommand
        {
            get { return new AsyncCommand(AddNewStopWordAsync); }
        }

        private async Task AddNewStopWordAsync(object parameter)
        {
            // Validation
            SystemNotification validateNotification;
            if (!IsNewStopWordModelValid(out validateNotification))
            {
                this.NewStopWordNotification = validateNotification;
                return;
            }

            try
            {
                StopWord newStopWord = await StopWord.CreateAsync(this.NewStopWord.Content);
                if (newStopWord != null)
                {
                    await LoadStopWordsAsync();
                    String statusMessage = String.Format("Новое стоп-слово '{0}' успешно добавлено.",
                        newStopWord.Content);
                    this.NewStopWordNotification = base.GetSuccessNotification(statusMessage);
                }
                else
                {
                    this.NewStopWordNotification =
                        base.GetErrorNotification("Что-то пошло не так. Не удалось добавить стоп-слово.");
                }
            }
            catch (DuplicateNameException e)
            {
                String exceptionMessage = String.Format("Стоп-слово '{0}' уже есть в базе.", this.NewStopWord.Content);
                this.NewStopWordNotification = base.GetErrorNotification(exceptionMessage);
            }
            catch (ArgumentNullException e)
            {
                String exceptionMessage = "Стоп-слово не может быть пустым.";
                this.NewStopWordNotification = base.GetErrorNotification(exceptionMessage);
            }
            catch (Exception e)
            {
                String exceptionMessage = "Произошло непредвиденное исключение. Попробуйте еще раз.";
                this.NewStopWordNotification = base.GetErrorNotification(exceptionMessage);
            }
        }

        #endregion

        #region Helpers

        private async Task LoadStopWordsAsync()
        {
            StopWordList stopWordList = await StopWordList.GetAllStopWordsAsync();
            List<StopWordModel> stopWordModels = stopWordList.Select(x => new StopWordModel
            {
                StopWordId = x.StopWordId,
                Content = x.Content
            }).ToList();

            this.StopWords = new ObservableCollection<StopWordModel>(stopWordModels);
        }

        private Boolean IsNewStopWordModelValid(out SystemNotification systemNotification)
        {
            systemNotification = null;
            if (String.IsNullOrEmpty(this.NewStopWord.Content) || String.IsNullOrEmpty(this.NewStopWord.Content.Trim()))
            {
                systemNotification = base.GetErrorNotification("Стоп-слово не может быть пустым.");
                return false;
            }

            if (this.NewStopWord.Content.Length > MaxStopWordLength)
            {
                String warningMessage = String.Format("Длина стоп-слова не должна превышать {0} символов.", MaxStopWordLength);
                systemNotification = base.GetWarningNotification(warningMessage);
                return false;
            }

            return true;
        }

        #endregion
    }
}