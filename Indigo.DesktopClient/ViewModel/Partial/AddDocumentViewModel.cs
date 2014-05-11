using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Indigo.BusinessLogicLayer.Analysis;
using Indigo.DesktopClient.CommandDelegates;
using Indigo.DesktopClient.Model;
using Indigo.DesktopClient.Model.DocumentAnalysis;
using Microsoft.Win32;

namespace Indigo.DesktopClient.ViewModel.Partial
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class AddDocumentViewModel : CommonPartialViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the AddDocumentViewModel class.
        /// </summary>
        public AddDocumentViewModel()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// The <see cref="SelectedDocumentModel" /> property's name.
        /// </summary>
        public const String DocumentAnalysisModelPropertyName = "SelectedDocumentModel";

        private DocumentAnalysisModel _selectedSelectedDocumentModel;

        /// <summary>
        /// Sets and gets the SelectedDocumentModel property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public DocumentAnalysisModel SelectedDocumentModel
        {
            get
            {
                return this._selectedSelectedDocumentModel;
            }

            set
            {
                if (this._selectedSelectedDocumentModel == value)
                {
                    return;
                }

                this._selectedSelectedDocumentModel = value;
                base.RaisePropertyChanged(DocumentAnalysisModelPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="IsDocumentSelected" /> property's name.
        /// </summary>
        public const String IsDocumentSelectedPropertyName = "IsDocumentSelected";

        private Boolean _isDocumentSelected = false;

        /// <summary>
        /// Sets and gets the IsDocumentSelected property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Boolean IsDocumentSelected
        {
            get
            {
                return this._isDocumentSelected;
            }

            set
            {
                if (this._isDocumentSelected == value)
                {
                    return;
                }

                this._isDocumentSelected = value;
                base.RaisePropertyChanged(IsDocumentSelectedPropertyName);
            }
        }

        #endregion

        #region Commands

        public ICommand LoadDocumentCommand
        {
            get
            {
                return new RelayCommand(LoadDocument);
            }
        }

        private void LoadDocument()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Microsoft Word Document (*.doc;*.docx)|*.doc;*.docx|OpenDocument Text (*.odt)|*.odt|Rich Text Format (*.rtf)|*.rtf|All files (*.doc;*.docx;*.odt;*.rtf)|*.doc;*.docx;*.odt;*.rtf";
            openFileDialog.FilterIndex = 1;

            openFileDialog.FileOk += (sender, args) =>
            {
                OpenFileDialog currentDialog = sender as OpenFileDialog;
                if (currentDialog != null && !String.IsNullOrEmpty(currentDialog.FileName) && currentDialog.FileName.Length > 0)
                {
                    FileInfo selectedFileInfo = new FileInfo(currentDialog.FileName);
                    this.SelectedDocumentModel = new DocumentAnalysisModel
                    {
                        FullName = selectedFileInfo.FullName,
                        FileName = selectedFileInfo.Name,
                        Directory = selectedFileInfo.DirectoryName,
                        Size = selectedFileInfo.Length,
                        Thumbnail = GetFileExtensionThumbnail(selectedFileInfo.Extension),
                        TempFileFullName = String.Concat(Guid.NewGuid().ToString("N"), selectedFileInfo.Extension)
                    };
                    this.IsDocumentSelected = true;
                }
                else
                {
                    MessageBox.Show("Выберите файл для анализа!");
                }
            };

            openFileDialog.ShowDialog();
        }

        public ICommand AnalysDocumentCommand
        {
            get
            {
                return new AsyncCommand(AnalysDocumentAsync);
            }
        }

        private async Task AnalysDocumentAsync(object o)
        {
            await Task.Delay(100);
        }

        #endregion

        #region Helpers

        private String GetFileExtensionThumbnail(String extension)
        {
            String thumbnailPath = String.Empty;
            String applicationResourcesDirectory = String.Concat(AppDomain.CurrentDomain.BaseDirectory, "Thumbnails\\");
            switch (extension)
            {
                case ".doc":
                    {
                        thumbnailPath = String.Concat(applicationResourcesDirectory, "Doc_Format.png");
                    } break;

                case ".docx":
                    {
                        thumbnailPath = String.Concat(applicationResourcesDirectory, "Docx_Format.png");
                    } break;

                case ".odt":
                    {
                        thumbnailPath = String.Concat(applicationResourcesDirectory, "Odt_Format.png");
                    } break;

                case ".rtf":
                    {
                        thumbnailPath = String.Concat(applicationResourcesDirectory, "Rtf_Format.png");
                    } break;
            }

            return thumbnailPath;
        }

        #endregion
    }
}