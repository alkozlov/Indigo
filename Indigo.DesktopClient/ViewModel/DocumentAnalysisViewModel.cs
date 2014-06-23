using System.Text;
using DevExpress.Xpf.RichEdit;
using DevExpress.XtraEditors.Controls.Rtf;
using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.Internal;
using DevExpress.XtraRichEdit.Model;
using GalaSoft.MvvmLight.Messaging;
using Indigo.DesktopClient.Helpers;
using Indigo.DesktopClient.Model.Notifications;
using CharacterStyleCollection = DevExpress.XtraRichEdit.API.Native.CharacterStyleCollection;
using RichEditControl = DevExpress.XtraRichEdit.RichEditControl;

namespace Indigo.DesktopClient.ViewModel
{
    using Microsoft.Win32;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;

    using GalaSoft.MvvmLight.Command;

    using Indigo.BusinessLogicLayer.Analysis;
    using Indigo.BusinessLogicLayer.Document;
    using Indigo.BusinessLogicLayer.Shingles;
    using Indigo.DesktopClient.CommandDelegates;
    using Indigo.DesktopClient.Model;
    using Indigo.DesktopClient.Model.DocumentAnalysis;
    using Indigo.DesktopClient.Model.PenthouseModels;

    /// <summary>
    /// This class contains properties that a View can data bind to.
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class DocumentAnalysisViewModel : CommonViewModel
    {
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

        /// <summary>
        /// The <see cref="AnalysisPanelSettings" /> property's name.
        /// </summary>
        public const string AnalysisPanelSettengsPropertyName = "AnalysisPanelSettings";

        private AnalysisSettings _analysisPanelSettings;

        /// <summary>
        /// Sets and gets the AnalysisPanelSettings property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public AnalysisSettings AnalysisPanelSettings
        {
            get
            {
                return this._analysisPanelSettings;
            }

            set
            {
                if (this._analysisPanelSettings == value)
                {
                    return;
                }

                this._analysisPanelSettings = value;
                base.RaisePropertyChanged(AnalysisPanelSettengsPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="CompareResltShingles" /> property's name.
        /// </summary>
        public const string CompareResltShinglesPropertyName = "CompareResltShingles";

        private ShinglesResult _compareResltShingles;

        /// <summary>
        /// Sets and gets the CompareResltShingles property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ShinglesResult CompareResltShingles
        {
            get
            {
                return this._compareResltShingles;
            }

            set
            {
                if (this._compareResltShingles == value)
                {
                    return;
                }

                this._compareResltShingles = value;
                base.RaisePropertyChanged(CompareResltShinglesPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="SimilarDocuments" /> property's name.
        /// </summary>
        public const string SimilarDocumentsPropertyName = "SimilarDocuments";

        private ObservableCollection<SimilarDocumentModel> _similarDocuments;

        /// <summary>
        /// Sets and gets the SimilarDocuments property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<SimilarDocumentModel> SimilarDocuments
        {
            get
            {
                return this._similarDocuments;
            }

            set
            {
                if (this._similarDocuments == value)
                {
                    return;
                }

                this._similarDocuments = value;
                base.RaisePropertyChanged(SimilarDocumentsPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="SelectedSimilarDocument" /> property's name.
        /// </summary>
        public const string SelectedSimilarDocumentPropertyName = "SelectedSimilarDocument";

        private SimilarDocumentModel _selectedSimilarDocument;

        /// <summary>
        /// Sets and gets the SelectedSimilarDocument property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public SimilarDocumentModel SelectedSimilarDocument
        {
            get
            {
                return this._selectedSimilarDocument;
            }

            set
            {
                if (this._selectedSimilarDocument == value)
                {
                    return;
                }

                this._selectedSimilarDocument = value;
                base.RaisePropertyChanged(SelectedSimilarDocumentPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="LsaMap" /> property's name.
        /// </summary>
        public const string LsaMapPropertyName = "LsaMap";

        private ObservableCollection<LsaModel> _lsaMap;

        /// <summary>
        /// Sets and gets the LsaMap property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<LsaModel> LsaMap
        {
            get
            {
                return this._lsaMap;
            }

            set
            {
                if (this._lsaMap == value)
                {
                    return;
                }

                this._lsaMap = value;
                base.RaisePropertyChanged(LsaMapPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="LsaSelectedDocument" /> property's name.
        /// </summary>
        public const string LsaSelectedDocumentPropertyName = "LsaSelectedDocument";

        private ObservableCollection<LsaModel> _lsaSelectedDocument;

        /// <summary>
        /// Sets and gets the LsaSelectedDocument property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<LsaModel> LsaSelectedDocument
        {
            get
            {
                return this._lsaSelectedDocument;
            }

            set
            {
                if (this._lsaSelectedDocument == value)
                {
                    return;
                }

                this._lsaSelectedDocument = value;
                base.RaisePropertyChanged(LsaSelectedDocumentPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="SelectedDocumentContent" /> property's name.
        /// </summary>
        public const string SelectedDocumentContentPropertyName = "SelectedDocumentContent";

        private RichEditDocumentContent _selectedDocumentContent;

        /// <summary>
        /// Sets and gets the SelectedDocumentContent property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public RichEditDocumentContent SelectedDocumentContent
        {
            get
            {
                return this._selectedDocumentContent;
            }

            set
            {
                if (this._selectedDocumentContent.Content == value.Content)
                {
                    return;
                }

                this._selectedDocumentContent = value;
                base.RaisePropertyChanged(SelectedDocumentContentPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="SelectedDocumentPlainText" /> property's name.
        /// </summary>
        public const string SelectedDocumentPlainTextPropertyName = "SelectedDocumentPlainText";

        private String _selectedDocumentPlainText = String.Empty;

        /// <summary>
        /// Sets and gets the SelectedDocumentPlainText property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public String SelectedDocumentPlainText
        {
            get
            {
                return this._selectedDocumentPlainText;
            }

            set
            {
                if (this._selectedDocumentPlainText == value)
                {
                    return;
                }

                this._selectedDocumentPlainText = value;
                base.RaisePropertyChanged(SelectedDocumentPlainTextPropertyName);
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

            openFileDialog.FileOk += async (sender, args) =>
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
                    this.IsDocumentSelected = true;
                    this.SimilarDocuments = new ObservableCollection<SimilarDocumentModel>();

                    // Load selected document plan text
                    this.SelectedDocumentPlainText = await DocumentContentHelper.GetDocumentPlainText(selectedFileInfo.FullName);
                    using (var documentModel = new DevExpress.XtraRichEdit.Model.DocumentModel())
                    {
                        var rtfConverter = new StringEditValueToDocumentModelConverter(DocumentFormat.PlainText, Encoding.Default);
                        var stringConverter = new DocumentModelToStringConverter(DocumentFormat.Rtf, Encoding.Default);

                        rtfConverter.ConvertToDocumentModel(documentModel, this.SelectedDocumentPlainText);
                        documentModel.DefaultCharacterProperties.FontName = "Tahoma";
                        documentModel.DefaultCharacterProperties.DoubleFontSize = 16;

                        String rtfString = stringConverter.ConvertToEditValue(documentModel) as String;
                        this.SelectedDocumentContent = new RichEditDocumentContent(DocumentFormat.Rtf, rtfString);
                    }
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
            AnalysisTargetDocument targetDocument = new AnalysisTargetDocument
            {
                FilePath = this.DocumentAnalysisModel.FullName
            };

            ShingleSize selectedShingleSize = (ShingleSize) this.AnalysisPanelSettings.ShingleSize;
            float minimalSimilarityLevel = Convert.ToSingle(this.AnalysisPanelSettings.MinimalSimilarityLevel) / 100;
            
            DocumentAnalysisSettings documentAnalysisSettings = new DocumentAnalysisSettings(selectedShingleSize, minimalSimilarityLevel);
            CompareResult compareResult = await DocumentAnalyzer.Current.FindSimilarDocumentsAsync(targetDocument, documentAnalysisSettings);
            this.CompareResltShingles = compareResult.ShinglesResult;

            if (compareResult.ShinglesResult.Count > 0)
            {
                List<SimilarDocumentModel> similarDocuments = new List<SimilarDocumentModel>();
                foreach (var shinglesResultSet in compareResult.ShinglesResult)
                {
                    SimilarDocumentModel similarDocumentModel = await this.ConvertCompareResultSetToModelObject(shinglesResultSet);
                    similarDocuments.Add(similarDocumentModel);
                }
                this.SimilarDocuments = new ObservableCollection<SimilarDocumentModel>(similarDocuments);

                var firstShinglesSet = compareResult.ShinglesResult.First();
                this.SelectedSimilarDocument = similarDocuments.First();
                SimilarDocumentsSearchNotification message = new SimilarDocumentsSearchNotification(firstShinglesSet);
                Messenger.Default.Send<SimilarDocumentsSearchNotification>(message, "1111");

                // LSA
                DocumentList documentList = await DocumentList.GetAllDocumentsAsync();
                List<LsaModel> lsaModels =
                    compareResult.LsaResult.Select(lsaResultSet => CreateLsaModel(lsaResultSet, documentList)).ToList();

                var lsaSelectedDocument = lsaModels.FirstOrDefault(x => x.DocumentId.HasValue && x.DocumentId.Equals(0));
                //lsaModels.Remove(lsaSelectedDocument);
                this.LsaSelectedDocument = new ObservableCollection<LsaModel>(new List<LsaModel> { lsaSelectedDocument });
                this.LsaMap = new ObservableCollection<LsaModel>(lsaModels);
            }
            else
            {
                this.SimilarDocuments = new ObservableCollection<SimilarDocumentModel>();
                this.LsaMap = new ObservableCollection<LsaModel>();
            }
        }

        public ICommand SelectSimilarDocumentCommand
        {
            get
            {
                return new RelayCommand<SimilarDocumentModel>(SelectSimilarDocument);
            }
        }

        private void SelectSimilarDocument(SimilarDocumentModel similarDocument)
        {
            if (similarDocument != null && this.CompareResltShingles.Any())
            {
                ShinglesResultSet selectedSimilarDocument =
                    this.CompareResltShingles.FirstOrDefault(x => x.DocumentId.Equals(similarDocument.DocumentId));
                if (selectedSimilarDocument != null)
                {
                    SimilarDocumentsSearchNotification message = new SimilarDocumentsSearchNotification(selectedSimilarDocument);
                    Messenger.Default.Send<SimilarDocumentsSearchNotification>(message, "1111");
                }
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the DocumentAnalysisViewModel class.
        /// </summary>
        public DocumentAnalysisViewModel()
        {
            this.AnalysisPanelSettings = base.GetDefaultShingleSize();
            this.SimilarDocuments = new ObservableCollection<SimilarDocumentModel>();
            this.LsaMap = new ObservableCollection<LsaModel>();
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

        private async Task<SimilarDocumentModel> ConvertCompareResultSetToModelObject(ShinglesResultSet shinglesResultSet)
        {
            Document document = await Document.GetAsync(shinglesResultSet.DocumentId);
            SimilarDocumentModel similarDocumentModel = null;
            if (document != null)
            {
                Int32 similarityValue = Convert.ToInt32(shinglesResultSet.MatchingCoefficient * 100);

                similarDocumentModel = new SimilarDocumentModel
                {
                    DocumentId = document.DocumentId,
                    DocumentName = document.OriginalFileName,
                    SimilarityValue = similarityValue,
                    DocumentType = GetDocumentType(document.OriginalFileName)
                };
            }

            return similarDocumentModel;
        }

        private DocumentType GetDocumentType(String fileName)
        {
            String fileExtansion = Path.GetExtension(fileName);
            DocumentType documentType = DocumentType.Doc;

            if (!String.IsNullOrEmpty(fileExtansion))
            {
                if (fileExtansion.ToLower().Equals(".doc"))
                {
                    documentType = DocumentType.Doc;
                }
                else if (fileExtansion.ToLower().Equals(".docx"))
                {
                    documentType = DocumentType.Docx;
                }
                else if (fileExtansion.ToLower().Equals(".odt"))
                {
                    documentType = DocumentType.Odt;
                }
                else if (fileExtansion.ToLower().Equals(".rtf"))
                {
                    documentType = DocumentType.Rtf;
                }
            }

            return documentType;
        }

        private LsaModel CreateLsaModel(LsaResultSet lsaResultSet, DocumentList documentList)
        {
            LsaModel lsaModel;
            if (lsaResultSet.DocumentId.HasValue)
            {
                var document = documentList.FirstOrDefault(x => x.DocumentId.Equals(lsaResultSet.DocumentId));
                lsaModel = new LsaModel
                {
                    DocumentId = lsaResultSet.DocumentId,
                    DocumentName = document != null ? document.OriginalFileName : "НЕ НАЙДЕН",
                    X = lsaResultSet.X,
                    Y = lsaResultSet.Y,
                    Radius = 15,
                    Brush = Brushes.Teal
                };
            }
            else
            {
                lsaModel = new LsaModel
                {
                    DocumentId = 0,
                    DocumentName = "ВЫБРАННЫЙ ДОКУМЕНТ",
                    X = lsaResultSet.X,
                    Y = lsaResultSet.Y,
                    Radius = 25,
                    Brush = Brushes.Tomato
                };
            }

            return lsaModel;
        }

        #endregion
    }
}