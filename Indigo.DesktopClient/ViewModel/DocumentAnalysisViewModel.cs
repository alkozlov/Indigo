namespace Indigo.DesktopClient.ViewModel
{
    using Microsoft.Win32;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;

    using GalaSoft.MvvmLight.Command;
    
    using Indigo.BusinessLogicLayer.Shingles;
    using Indigo.DesktopClient.CommandDelegates;
    using Indigo.DesktopClient.Model;
    using Indigo.DesktopClient.View;
    using Indigo.Tools;
    using Indigo.Tools.Converters;
    using Indigo.Tools.Parsers;

    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class DocumentAnalysisViewModel : CommonViewModel
    {
        private const String TempApplicationFolder = @"Indigo\\";
        private const String TempLematizationFilePostfix = "_LEM";

        #region Properties

        /// <summary>
        /// The <see cref="DocumentAnalysisModel" /> property's name.
        /// </summary>
        public const String DocumentAnalysisModelPropertyName = "DocumentAnalysisModel";

        private DocumentAnalysisModel _documentModel;

        /// <summary>
        /// Sets and gets the DocumentAnalysisModel property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public DocumentAnalysisModel DocumentAnalysisModel
        {
            get
            {
                return this._documentModel;
            }

            set
            {
                if (this._documentModel == value)
                {
                    return;
                }

                this._documentModel = value;
                base.RaisePropertyChanged(DocumentAnalysisModelPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="IsDocumentSelected" /> property's name.
        /// </summary>
        public const String IsDocumentSelectedPropertyName = "IsDocumentSelected";

        private Visibility _isDocumentSelected = Visibility.Collapsed;

        /// <summary>
        /// Sets and gets the IsDocumentSelected property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Visibility IsDocumentSelected
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

        public ICommand LoadDocumentComand
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
                    this.DocumentAnalysisModel = new DocumentAnalysisModel
                    {
                        FullName = selectedFileInfo.FullName,
                        FileName = selectedFileInfo.Name,
                        Directory = selectedFileInfo.DirectoryName,
                        Size = selectedFileInfo.Length,
                        Thumbnail = GetFileExtensionThumbnail(selectedFileInfo.Extension),
                        TempFileFullName = String.Concat(Guid.NewGuid().ToString("N"), selectedFileInfo.Extension)
                    };
                    this.IsDocumentSelected = Visibility.Visible;
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
                return new AsyncCommand(AnalysDocuments);
            }
        }

        private async Task AnalysDocuments(object o)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            // Shingles method
            // 1. Use lemmatization tool for get new file with normalized words
            // 2. Parse file

            // 1.
            String currentUserTempFolder = Path.GetTempPath();
            String outputDirectory = String.Concat(currentUserTempFolder, TempApplicationFolder);
            String tempFileOnlyName = Guid.NewGuid().ToString("N");
            String tempTextFileName = String.Format("{0}.txt", tempFileOnlyName);
            String tempTextFileFullName = String.Concat(outputDirectory, tempTextFileName);

            using (IDocumentConverter converter = new MsWordDocumentConverter())
            {
                await converter.ConvertDocumentToTextFileAsync(this.DocumentAnalysisModel.FullName, tempTextFileFullName);
            }

            String tempLematizationFileName = String.Format("{0}{1}.txt", tempFileOnlyName, TempLematizationFilePostfix);
            String tempLematizationFileFullName = String.Concat(outputDirectory, tempLematizationFileName);
            await LematizationTool.Current.ProcessDocumntAsync(tempTextFileFullName, tempLematizationFileFullName);

            // 2.
            var words = (await ParserTool.Current.ParseFileAsync(tempLematizationFileFullName)).ToList();
            ShingleList shingles = await ShingleList.CreateAsync(words, new List<string>(), 7);

            stopwatch.Stop();
            MessageBox.Show(stopwatch.ElapsedMilliseconds.ToString());
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the DocumentAnalysisViewModel class.
        /// </summary>
        public DocumentAnalysisViewModel()
        {
            
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