using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Indigo.BusinessLogicLayer.Analysis;
using Indigo.BusinessLogicLayer.Document;
using Indigo.DesktopClient.CommandDelegates;
using Indigo.DesktopClient.Model;
using Indigo.DesktopClient.Model.DocumentAnalysis;
using Indigo.DesktopClient.Model.PenthouseModels;
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
        private const Int32 DefaultWordfFequencyMinimalLimit = 2;

        private event EventHandler InitializeView;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the AddDocumentViewModel class.
        /// </summary>
        public AddDocumentViewModel()
        {
            this.WordfFequencyMinimalLimit = DefaultWordfFequencyMinimalLimit;
            this.AnalysResult = new AnalysisResult();

            this.InitializeView += async (sender, args) =>
            {
                await LoadSubjectsAsync();
            };
            this.InitializeView(this, null);
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
        /// The <see cref="SelectedSubject" /> property's name.
        /// </summary>
        public const string SelectedSubjectPropertyName = "SelectedSubject";

        private SubjectModel _selectedSubject;

        /// <summary>
        /// Sets and gets the SelectedSubject property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public SubjectModel SelectedSubject
        {
            get
            {
                return this._selectedSubject;
            }

            set
            {
                if (this._selectedSubject == value)
                {
                    return;
                }

                this._selectedSubject = value;
                base.RaisePropertyChanged(SelectedSubjectPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="WordfFequencyMinimalLimit" /> property's name.
        /// </summary>
        public const string WordfFequencyMinimalLimitPropertyName = "WordfFequencyMinimalLimit";

        private Int32 _wordfFequencyMinimalLimit;

        /// <summary>
        /// Sets and gets the WordfFequencyMinimalLimit property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public Int32 WordfFequencyMinimalLimit
        {
            get
            {
                return this._wordfFequencyMinimalLimit;
            }

            set
            {
                if (this._wordfFequencyMinimalLimit == value)
                {
                    return;
                }

                this._wordfFequencyMinimalLimit = value;
                base.RaisePropertyChanged(WordfFequencyMinimalLimitPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="AnalysResult" /> property's name.
        /// </summary>
        public const string AnalysResultPropertyName = "AnalysResult";

        private AnalysisResult _analysisResult;

        /// <summary>
        /// Sets and gets the AnalysResult property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public AnalysisResult AnalysResult
        {
            get
            {
                return this._analysisResult;
            }

            set
            {
                if (this._analysisResult == value)
                {
                    return;
                }

                this._analysisResult = value;
                base.RaisePropertyChanged(AnalysResultPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="KeyWords" /> property's name.
        /// </summary>
        public const string KeyWordsPropertyName = "KeyWords";

        private ObservableCollection<KeyWordModel> _keyWords;

        /// <summary>
        /// Sets and gets the KeyWords property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<KeyWordModel> KeyWords
        {
            get
            {
                return this._keyWords;
            }

            set
            {
                if (this._keyWords == value)
                {
                    return;
                }

                this._keyWords = value;
                base.RaisePropertyChanged(KeyWordsPropertyName);
            }
        }

        /// <summary>
        /// The <see cref="TargetSubject" /> property's name.
        /// </summary>
        public const string TargetSubjectPropertyName = "TargetSubject";

        private SubjectModel _targetSubject;

        /// <summary>
        /// Sets and gets the TargetSubject property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public SubjectModel TargetSubject
        {
            get
            {
                return this._targetSubject;
            }

            set
            {
                if (this._targetSubject == value)
                {
                    return;
                }

                this._targetSubject = value;
                base.RaisePropertyChanged(TargetSubjectPropertyName);
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
            AnalysisTargetDocument targetDocument = new AnalysisTargetDocument
            {
                FilePath = this.SelectedDocumentModel.FullName
            };

            AnalysisResult analysisResult = await DocumentAnalyzer.Current.AnalyzeDocumentAsync(targetDocument);
            this.AnalysResult = analysisResult;
            this.FillKeyWords(this.AnalysResult.DocumentKeyWords);
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

        private async Task LoadSubjectsAsync()
        {
            SubjectList subjectList = await SubjectList.GetSubjectsAsync();
            List<SubjectModel> subjectModels = subjectList.OrderBy(x => x.SubjectHeader).Select(x => new SubjectModel
            {
                SubjectId = x.SubjectId,
                SubjectHeader = x.SubjectHeader
            }).ToList();

            this.Subjects = new ObservableCollection<SubjectModel>(subjectModels);

            if (this.SelectedSubject == null)
            {
                this.SelectedSubject = this.Subjects.FirstOrDefault();
            }
        }

        private void FillKeyWords(DocumentKeyWordList documentKeyWordList)
        {
            var keyWordsList = documentKeyWordList.Select(x => new KeyWordModel
            {
                Word = x.Word,
                Count = x.Usages
            });
            this.KeyWords = new ObservableCollection<KeyWordModel>(keyWordsList);
        }

        #endregion
    }
}