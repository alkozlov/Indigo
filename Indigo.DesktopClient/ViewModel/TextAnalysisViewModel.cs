using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using DevExpress.Xpf.RichEdit;
using DevExpress.XtraEditors.Controls.Rtf;
using DevExpress.XtraRichEdit;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Messaging;
using Indigo.BusinessLogicLayer.Analysis;
using Indigo.DesktopClient.CommandDelegates;
using Indigo.DesktopClient.Helpers;
using Indigo.DesktopClient.Model;
using Indigo.DesktopClient.Model.DocumentAnalysis;
using Indigo.DesktopClient.Model.Notifications;
using Indigo.DesktopClient.View;
using Microsoft.Win32;

namespace Indigo.DesktopClient.ViewModel
{
    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class TextAnalysisViewModel : CommonViewModel
    {
        #region Properties

        /// <summary>
        /// The <see cref="DocumentAnalysisModel1" /> property's name.
        /// </summary>
        public const String DocumentAnalysisModel1PropertyName = "DocumentAnalysisModel1";

        private DocumentAnalysisModel _documentModel1;

        /// <summary>
        /// Sets and gets the DocumentAnalysisModel property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public DocumentAnalysisModel DocumentAnalysisModel1
        {
            get
            {
                return this._documentModel1;
            }

            set
            {
                if (this._documentModel1 == value)
                {
                    return;
                }

                this._documentModel1 = value;
                base.RaisePropertyChanged(DocumentAnalysisModel1PropertyName);
            }
        }

        /// <summary>
        /// The <see cref="DocumentAnalysisModel2" /> property's name.
        /// </summary>
        public const String DocumentAnalysisModel2PropertyName = "DocumentAnalysisModel2";

        private DocumentAnalysisModel _documentModel2;

        /// <summary>
        /// Sets and gets the DocumentAnalysisModel property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public DocumentAnalysisModel DocumentAnalysisModel2
        {
            get
            {
                return this._documentModel2;
            }

            set
            {
                if (this._documentModel2 == value)
                {
                    return;
                }

                this._documentModel2 = value;
                base.RaisePropertyChanged(DocumentAnalysisModel2PropertyName);
            }
        }

        /// <summary>
        /// The <see cref="IsDocumentsSelected" /> property's name.
        /// </summary>
        public const String IsDocumentsSelectedPropertyName = "IsDocumentsSelected";

        private Boolean _isDocumentsSelected = false;

        /// <summary>
        /// Sets and gets the IsDocumentSelected property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Boolean IsDocumentsSelected
        {
            get
            {
                return this._isDocumentsSelected;
            }

            set
            {
                if (this._isDocumentsSelected == value)
                {
                    return;
                }

                this._isDocumentsSelected = value;
                base.RaisePropertyChanged(IsDocumentsSelectedPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="SelectedDocument1PlainText" /> property's name.
        /// </summary>
        public const string SelectedDocument1PlainTextPropertyName = "SelectedDocument1PlainText";

        private String _selectedDocument1PlainText = String.Empty;

        /// <summary>
        /// Sets and gets the SelectedDocumentPlainText property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public String SelectedDocument1PlainText
        {
            get
            {
                return this._selectedDocument1PlainText;
            }

            set
            {
                if (this._selectedDocument1PlainText == value)
                {
                    return;
                }

                this._selectedDocument1PlainText = value;
                base.RaisePropertyChanged(SelectedDocument1PlainTextPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="SelectedDocument2PlainText" /> property's name.
        /// </summary>
        public const string SelectedDocument2PlainTextPropertyName = "SelectedDocument2PlainText";

        private String _selectedDocument2PlainText = String.Empty;

        /// <summary>
        /// Sets and gets the SelectedDocumentPlainText property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public String SelectedDocument2PlainText
        {
            get
            {
                return this._selectedDocument2PlainText;
            }

            set
            {
                if (this._selectedDocument2PlainText == value)
                {
                    return;
                }

                this._selectedDocument2PlainText = value;
                base.RaisePropertyChanged(SelectedDocument2PlainTextPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="SelectedDocument1Content" /> property's name.
        /// </summary>
        public const string SelectedDocument1ContentPropertyName = "SelectedDocument1Content";

        private RichEditDocumentContent _selectedDocument1Content;

        /// <summary>
        /// Sets and gets the SelectedDocumentContent property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public RichEditDocumentContent SelectedDocument1Content
        {
            get
            {
                return this._selectedDocument1Content;
            }

            set
            {
                if (this._selectedDocument1Content.Content == value.Content)
                {
                    return;
                }

                this._selectedDocument1Content = value;
                base.RaisePropertyChanged(SelectedDocument1ContentPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="SelectedDocument2Content" /> property's name.
        /// </summary>
        public const string SelectedDocument2ContentPropertyName = "SelectedDocument2Content";

        private RichEditDocumentContent _selectedDocument2Content;

        /// <summary>
        /// Sets and gets the SelectedDocumentContent property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public RichEditDocumentContent SelectedDocument2Content
        {
            get
            {
                return this._selectedDocument2Content;
            }

            set
            {
                if (this._selectedDocument2Content.Content == value.Content)
                {
                    return;
                }

                this._selectedDocument2Content = value;
                base.RaisePropertyChanged(SelectedDocument2ContentPropertyName);
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the TextAnalysisViewModel class.
        /// </summary>
        public TextAnalysisViewModel()
        {
        }

        #endregion

        #region Commands

        public ICommand LoadDocumentsCommand
        {
            get
            {
                return new RelayCommand(LoadDocuments);
            }
        }

        private void LoadDocuments()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Microsoft Word Document (*.doc;*.docx)|*.doc;*.docx|OpenDocument Text (*.odt)|*.odt|Rich Text Format (*.rtf)|*.rtf|All files (*.doc;*.docx;*.odt;*.rtf)|*.doc;*.docx;*.odt;*.rtf";
            openFileDialog.FilterIndex = 1;
            openFileDialog.Multiselect = true;

            openFileDialog.FileOk += async (sender, args) =>
            {
                OpenFileDialog currentDialog = sender as OpenFileDialog;
                if (currentDialog != null && !String.IsNullOrEmpty(currentDialog.FileName) && currentDialog.FileNames.Length == 2)
                {
                    String file1Name = currentDialog.FileNames[0];
                    String file2Name = currentDialog.FileNames[1];
                    this.DocumentAnalysisModel1 = this.GetDocumentAnalysisModel(file1Name);
                    this.DocumentAnalysisModel2 = this.GetDocumentAnalysisModel(file2Name);
                    this.IsDocumentsSelected = true;

                    // Load selected document 1 plan text
                    this.SelectedDocument1PlainText = await DocumentContentHelper.GetDocumentPlainText(file1Name);
                    using (var documentModel = new DevExpress.XtraRichEdit.Model.DocumentModel())
                    {
                        var rtfConverter = new StringEditValueToDocumentModelConverter(DocumentFormat.PlainText, Encoding.Default);
                        var stringConverter = new DocumentModelToStringConverter(DocumentFormat.Rtf, Encoding.Default);

                        rtfConverter.ConvertToDocumentModel(documentModel, this.SelectedDocument1PlainText);
                        documentModel.DefaultCharacterProperties.FontName = "Tahoma";
                        documentModel.DefaultCharacterProperties.DoubleFontSize = 16;

                        String rtfString = stringConverter.ConvertToEditValue(documentModel) as String;
                        this.SelectedDocument1Content = new RichEditDocumentContent(DocumentFormat.Rtf, rtfString);
                    }

                    // Load selected document 2 plan text
                    this.SelectedDocument2PlainText = await DocumentContentHelper.GetDocumentPlainText(file2Name);
                    using (var documentModel = new DevExpress.XtraRichEdit.Model.DocumentModel())
                    {
                        var rtfConverter = new StringEditValueToDocumentModelConverter(DocumentFormat.PlainText, Encoding.Default);
                        var stringConverter = new DocumentModelToStringConverter(DocumentFormat.Rtf, Encoding.Default);

                        rtfConverter.ConvertToDocumentModel(documentModel, this.SelectedDocument2PlainText);
                        documentModel.DefaultCharacterProperties.FontName = "Tahoma";
                        documentModel.DefaultCharacterProperties.DoubleFontSize = 16;

                        String rtfString = stringConverter.ConvertToEditValue(documentModel) as String;
                        this.SelectedDocument2Content = new RichEditDocumentContent(DocumentFormat.Rtf, rtfString);
                    }
                }
                else
                {
                    MessageBox.Show("Выберите 2 файла для анализа!");
                }
            };

            openFileDialog.ShowDialog();
        }

        public ICommand AnalysDocumentsCommand
        {
            get
            {
                return new AsyncCommand(AnalysDocumentsAsync);
            }
        }

        private async Task AnalysDocumentsAsync(object o)
        {
            if (this.IsDocumentsSelected)
            {
                AnalysisTargetDocument targetDocument1 = new AnalysisTargetDocument
                {
                    FilePath = this.DocumentAnalysisModel1.FullName
                };
                AnalysisTargetDocument targetDocument2 = new AnalysisTargetDocument
                {
                    FilePath = this.DocumentAnalysisModel2.FullName
                };

                CompareResult result = await DocumentAnalyzer.Current.CompareDocumentsAsync(targetDocument1, targetDocument2);
                if (result != null && result.ShinglesResult != null && result.ShinglesResult.Count == 2 &&
                    result.ShinglesResult[0].MatchingCoefficient > 0 || result.ShinglesResult[1].MatchingCoefficient > 0)
                {
                    // First similar shingles
                    ShinglesResultSet document1SimilarShingles = result.ShinglesResult.First();
                    // Second similar shingles
                    ShinglesResultSet document2SimilarShingles = result.ShinglesResult.Last();

                    SimilarDocumentsSearchNotification message1 = new SimilarDocumentsSearchNotification(document1SimilarShingles);
                    SimilarDocumentsSearchNotification message2 = new SimilarDocumentsSearchNotification(document2SimilarShingles);
                    Messenger.Default.Send<SimilarDocumentsSearchNotification>(message1, "1");
                    Messenger.Default.Send<SimilarDocumentsSearchNotification>(message2, "2");
                }
                else
                {
                    MessageBox.Show("Проверка не выявила схожих элементов в текстах.");
                }
            }
            else
            {
                MessageBox.Show("Вы не выбрали 2 документа для анализа!");
            }
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

        private DocumentAnalysisModel GetDocumentAnalysisModel(String fileName)
        {
            FileInfo selectedFileInfo = new FileInfo(fileName);
            DocumentAnalysisModel documentAnalysisModel = new DocumentAnalysisModel
            {
                FullName = selectedFileInfo.FullName,
                FileName = selectedFileInfo.Name,
                Directory = selectedFileInfo.DirectoryName,
                Size = selectedFileInfo.Length,
                Thumbnail = GetFileExtensionThumbnail(selectedFileInfo.Extension),
                TempFileFullName = String.Concat(Guid.NewGuid().ToString("N"), selectedFileInfo.Extension)
            };

            return documentAnalysisModel;
        }

        #endregion
    }
}