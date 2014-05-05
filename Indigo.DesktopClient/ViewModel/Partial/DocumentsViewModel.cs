namespace Indigo.DesktopClient.ViewModel.Partial
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows.Input;

    using GalaSoft.MvvmLight.Command;

    using Infragistics.Controls.Grids;

    using Indigo.BusinessLogicLayer.Document;
    using Indigo.BusinessLogicLayer.Shingles;
    using Indigo.BusinessLogicLayer.Storage;
    using Indigo.DesktopClient.CommandDelegates;
    using Indigo.DesktopClient.Model.Notifications;
    using Indigo.DesktopClient.Model.PenthouseModels;
    using Indigo.DesktopClient.View;

    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class DocumentsViewModel : CommonPartialViewModel
    {
        private const String DocumentFieldKey = "DocumentGuid";

        private event EventHandler InitializeViewModelEvent;

        #region Properties

        /// <summary>
        /// The <see cref="Documents" /> property's name.
        /// </summary>
        public const string DocumentsPropertyName = "Documents";

        private ObservableCollection<DocumentModel> _documents;

        /// <summary>
        /// Sets and gets the Documents property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<DocumentModel> Documents
        {
            get
            {
                return this._documents;
            }

            set
            {
                if (this._documents == value)
                {
                    return;
                }

                this._documents = value;
                base.RaisePropertyChanged(DocumentsPropertyName);
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

        public ICommand AddDocumentsCommand
        {
            get
            {
                return new RelayCommand(AddDocuments);
            }
        }

        private void AddDocuments()
        {
            base.SendNavigationMessage(ApplicationView.AddDocuments, NavigationToken.AddDocumentsToken);
        }

        public ICommand DeleteSelectedDocumentsCommand
        {
            get
            {
                return new AsyncCommand(DeleteSelectedDocuments);
            }
        }

        private async Task DeleteSelectedDocuments(object parameter)
        {
            await Task.Delay(100);

            SelectedRowsCollection selectedRows = parameter as SelectedRowsCollection;
            if (selectedRows != null && selectedRows.Count > 0)
            {
                try
                {
                    Int32 removedRecords = 0;
                    foreach (var selectedRow in selectedRows)
                    {
                        var documentKeyColumn = selectedRow.Columns[DocumentFieldKey];
                        if (documentKeyColumn != null)
                        {
                            Guid documentGuid = Guid.Parse(selectedRow.Cells[documentKeyColumn.Key].Value.ToString());
                            Document document = await Document.GetAsync(documentGuid);
                            if (document != null)
                            {
                                // Remove subject from database
                                await document.DeleteAsync();
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
                    await LoadDocumentsAsync();
                }
                catch (Exception e)
                {
                    String errorMessage = "Произошло непредвиденное исключение. Попробуйте еще раз.";
                    this.ToolbarNotification = base.GetErrorNotification(errorMessage);
                }
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the DocumentsViewModel class.
        /// </summary>
        public DocumentsViewModel()
        {
            this.InitializeViewModelEvent += async (sender, args) =>
            {
                await LoadDocumentsAsync();
            };

            this.InitializeViewModelEvent(this, null);
        }

        #endregion

        #region Helpers

        private async Task LoadDocumentsAsync()
        {
            DocumentList documents = await DocumentList.GetAllDocumentsAsync();

            List<DocumentModel> documentModels = new List<DocumentModel>();
            var availableShingleSizes = Enum.GetValues(typeof(AnalysisAccuracy)).Cast<byte>().ToArray();
            foreach (var document in documents)
            {
                // Get common document info
                DocumentModel documentModel = new DocumentModel
                {
                    DocumentGuid = document.DocumentGuid,
                    DocumentFileName = document.OriginalFileName,
                    StorageFileName = document.StoredFileName,
                    AddedByUser = document.AddedByUser != null ? document.AddedByUser.Login : "USER DELETED"
                };

                // Get document info from storage
                using (StorageConnection storageConnection = StorageConnector.GetStorageConnection(StorageType.Local))
                {
                    if (storageConnection != null)
                    {
                        FileInfo storageFileInfo = storageConnection.GetFileInfo(document.StoredFileName);
                        if (storageFileInfo != null && storageFileInfo.Exists)
                        {
                            documentModel.Size = storageFileInfo.Length;
                            documentModel.IsCorrupted = false;
                        }
                        else
                        {
                            documentModel.Size = 0;
                            documentModel.IsCorrupted = true;
                        }
                    }
                }

                // Get shingles infrmation
                Dictionary<Int32, Int32> documentShinglesStatistic = new Dictionary<Int32, Int32>();
                foreach (var shingleSize in availableShingleSizes)
                {
                    Int32 shinglesCount = await ShingleList.GetShinglesCountAsync(document.DocumentId, shingleSize);
                    documentShinglesStatistic.Add(shingleSize, shinglesCount);
                }
                documentModel.ShinglesStatistic = documentShinglesStatistic;

                documentModels.Add(documentModel);
            }

            this.Documents = new ObservableCollection<DocumentModel>(documentModels);
        }

        #endregion
    }
}