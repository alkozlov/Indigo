using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Indigo.DesktopClient.CommandDelegates;
using Indigo.DesktopClient.Helpers;
using Indigo.DesktopClient.Model.PenthouseModels;
using Indigo.DesktopClient.View;
using Microsoft.Office.Interop.Word;
using Microsoft.Win32;
using Task = System.Threading.Tasks.Task;

namespace Indigo.DesktopClient.ViewModel.Partial
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class AddDocumentsViewModel : CommonPartialViewModel
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the AddDocumentsViewModel class.
        /// </summary>
        public AddDocumentsViewModel()
        {
            this.SelectedDocumets = new ObservableCollection<NewDocumentModel>();
        }

        #endregion

        #region Overrides

        public override ApplicationView ViewType
        {
            get { return ApplicationView.AddDocuments; }
        }

        #endregion

        #region Properties

        /// <summary>
        /// The <see cref="SelectedDocumets" /> property's name.
        /// </summary>
        public const string SelectedDocumetsPropertyName = "SelectedDocumets";

        private ObservableCollection<NewDocumentModel> _selectedDocumets;

        /// <summary>
        /// Sets and gets the SelectedDocumets property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<NewDocumentModel> SelectedDocumets
        {
            get
            {
                return this._selectedDocumets;
            }

            set
            {
                if (this._selectedDocumets == value)
                {
                    return;
                }

                this._selectedDocumets = value;
                base.RaisePropertyChanged(SelectedDocumetsPropertyName);
            }
        }

        #endregion

        public ICommand SelectDocumentsCommand
        {
            get
            {
                return new RelayCommand(SelectDocuments);
            }
        }

        private void SelectDocuments()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Microsoft Word Document (*.doc;*.docx)|*.doc;*.docx|OpenDocument Text (*.odt)|*.odt|Rich Text Format (*.rtf)|*.rtf|All files (*.doc;*.docx;*.odt;*.rtf)|*.doc;*.docx;*.odt;*.rtf";
            openFileDialog.FilterIndex = 1;
            openFileDialog.Multiselect = true;

            openFileDialog.FileOk += async (sender, args) =>
            {
                OpenFileDialog currentOpenFileDialog = sender as OpenFileDialog;
                if (currentOpenFileDialog != null)
                {
                    String[] selectedFiles = currentOpenFileDialog.FileNames;
                    if (selectedFiles.Length > 0)
                    {
                        List<NewDocumentModel> selectedDocuments = selectedFiles.Select(
                            selectedFile => new FileInfo(selectedFile))
                            .Where(selectedFileInfo => selectedFileInfo.Exists)
                            .Select(selectedFileInfo => new NewDocumentModel
                            {
                                DocumentFullName = selectedFileInfo.FullName,
                                Size = selectedFileInfo.Length,
                                Thumbnail = GetFileExtensionThumbnail(selectedFileInfo.Extension)
                            }).ToList();

                        this.SelectedDocumets = new ObservableCollection<NewDocumentModel>(selectedDocuments);
                    }
                }
            };

            openFileDialog.ShowDialog();
        }

        public ICommand ProcessDocumentsAsyncCommand
        {
            get
            {
                return new AsyncCommand(ProcessDocumentsAsync);
            }
        }

        private async Task ProcessDocumentsAsync(object o)
        {
            var selectedFiles = this.SelectedDocumets.Select(x => x.DocumentFullName).ToList();

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            await DocumentProcessor.Current.ProcessDocumentsAsync(selectedFiles);
            stopwatch.Stop();
            Debug.WriteLine("==========> Время обработки документа: {0} мс", stopwatch.ElapsedMilliseconds);
        }

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