using System;
using System.Collections.ObjectModel;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Indigo.BusinessLogicLayer.Document;
using Indigo.DesktopClient.Model;
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
    public class DocumentsViewModel : CommonPartialViewModel
    {
        private const String StorageDirectoryName = "\\Storage";

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

            base.NavigateAction(this.ViewType, ApplicationView.Penthouse, NotificationTokens.AddDocumentsToken);
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
                String storagePath = String.Concat(AppDomain.CurrentDomain.BaseDirectory, StorageDirectoryName);
                DocumentList documents = await DocumentList.GetAllDocumentsAsync(storagePath);

                this.Documents = new ObservableCollection<DocumentModel>();
                foreach (var document in documents)
                {
                    DocumentModel documentModel = new DocumentModel
                    {
                        FileName = document.OriginalFileName,
                        AddedUserName = document.AddedByUser.Login,
                        FileIconPath = String.Empty,
                        StatusIconPath = String.Empty
                    };

                    this.Documents.Add(documentModel);
                }
            };

            this.InitializeViewModelEvent(this, null);
        }

        #endregion

        #region Overrides

        public override ApplicationView ViewType
        {
            get { return ApplicationView.DocumentsDatabase; }
        }

        #endregion
    }
}