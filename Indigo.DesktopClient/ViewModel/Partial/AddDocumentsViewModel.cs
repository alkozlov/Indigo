namespace Indigo.DesktopClient.ViewModel.Partial
{
    using Microsoft.Win32;
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Diagnostics;
    using System.IO;
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Windows;
    using System.Windows.Input;

    using GalaSoft.MvvmLight.Command;

    using Indigo.DesktopClient.CommandDelegates;
    using Indigo.DesktopClient.Helpers;
    using Indigo.DesktopClient.Model.PenthouseModels;

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
            this.ConsoleMessages = new ObservableCollection<ConsoleMessage>();
            this.CpuStatistic = new CpuStatistic();

            this.BeginMonitoringCpu();

            // Subscribe to document processor events
            DocumentProcessor.Current.OnDocumentProcessing += OnDocumentProcessingHandler;
            DocumentProcessor.Current.OnDocumentProcessed += OnDocumentProcessedHandler;
            DocumentProcessor.Current.OnDocumentProcessError += OnDocumentProcessErrorHandler;
            DocumentProcessor.Current.OnDocumentsProcessing += OnDocumentsProcessingHandler;
            DocumentProcessor.Current.OnDocumentsProcessed += DocumentsProcessedHandler;
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

        /// <summary>
        /// The <see cref="ConsoleMessages" /> property's name.
        /// </summary>
        public const string ConsoleMessagePropertyName = "ConsoleMessages";

        private ObservableCollection<ConsoleMessage> _consoleMessageses;

        /// <summary>
        /// Sets and gets the ConsoleMessages property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public ObservableCollection<ConsoleMessage> ConsoleMessages
        {
            get
            {
                return this._consoleMessageses;
            }

            set
            {
                if (this._consoleMessageses == value)
                {
                    return;
                }

                this._consoleMessageses = value;
                base.RaisePropertyChanged(ConsoleMessagePropertyName);
            }
        }

        /// <summary>
        /// The <see cref="CpuStatistic" /> property's name.
        /// </summary>
        public const string CpuStatisticPropertyName = "CpuStatistic";

        private CpuStatistic _cpuStatistic;

        /// <summary>
        /// Sets and gets the CpuStatistic property.
        /// Changes to that property's value raise the PropertyChanged event. 
        /// </summary>
        public CpuStatistic CpuStatistic
        {
            get
            {
                return this._cpuStatistic;
            }

            set
            {
                if (this._cpuStatistic == value)
                {
                    return;
                }

                this._cpuStatistic = value;
                base.RaisePropertyChanged(CpuStatisticPropertyName);
            }
        }

        #endregion

        #region Commands

        public ICommand SelectDocumentsCommand
        {
            get { return new RelayCommand(SelectDocuments); }
        }

        private void SelectDocuments()
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter =
                "Microsoft Word Document (*.doc;*.docx)|*.doc;*.docx|OpenDocument Text (*.odt)|*.odt|Rich Text Format (*.rtf)|*.rtf|All files (*.doc;*.docx;*.odt;*.rtf)|*.doc;*.docx;*.odt;*.rtf";
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
                                ProcessingStatus = ProcessingStatus.Begin
                            }).ToList();

                        this.SelectedDocumets = new ObservableCollection<NewDocumentModel>(selectedDocuments);
                    }
                }
            };

            openFileDialog.ShowDialog();
        }

        public ICommand ProcessDocumentsAsyncCommand
        {
            get { return new AsyncCommand(ProcessDocumentsAsync); }
        }

        private async Task ProcessDocumentsAsync(object o)
        {
            var selectedFiles = this.SelectedDocumets.Select(x => x.DocumentFullName).ToList();
            await DocumentProcessor.Current.ProcessDocumentsAsync(selectedFiles);
        }

        #endregion

        #region Event Handlers

        private void OnDocumentProcessingHandler(object sender, DocumentProcessingEventArgs documentProcessingEventArgs)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                String message = String.Format("Началась обработка документа '{0}'.", documentProcessingEventArgs.DocumentName);
                ConsoleMessage consoleMessage = ConsoleMessageHelper.GetWarningMessage(message);
                this.ConsoleMessages.Add(consoleMessage);
            });

            //foreach (var document in SelectedDocumets)
            //{
            //    if (document.DocumentFullName.Equals(documentProcessingEventArgs.DocumentName))
            //    {
            //        App.Current.Dispatcher.Invoke(() =>
            //        {
            //            document.ProcessingStatus = ProcessingStatus.InProgress;
            //            String message = String.Format("Началась обработка документа '{0}'.",
            //                documentProcessingEventArgs.DocumentName);
            //            this.DocumentsProcessedNotiication = base.GetWarningNotification(message);
            //        });
            //    }
            //}
        }

        private void OnDocumentProcessErrorHandler(object sender, DocumentProcessErrorEventArgs documentProcessErrorEventArgs)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                String message =
                    String.Format("Во время обработки документа '{0}' произошла ошибка.", documentProcessErrorEventArgs.DocumentName);
                ConsoleMessage consoleMessage = ConsoleMessageHelper.GetErrorMessage(message);
                this.ConsoleMessages.Add(consoleMessage);
            });
        }

        private void OnDocumentProcessedHandler(object sender, DocumentProcessedEventArgs documentProcessedEventArgs)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                NewDocumentModel document =
                    this.SelectedDocumets.FirstOrDefault(x => x.DocumentFullName.Equals(documentProcessedEventArgs.DocumentName));
                if (document != null)
                {
                    this.SelectedDocumets.Remove(document);
                    
                    String message = String.Format("Обработка документа '{0}' успешно завершена.",
                        documentProcessedEventArgs.DocumentName);
                    ConsoleMessage consoleMessage = ConsoleMessageHelper.GetSuccessMessage(message);
                    this.ConsoleMessages.Add(consoleMessage);

                    Int32 remainingDocumentsCount = this.SelectedDocumets.Count;
                    String totalDocumentsMessage;
                    if (remainingDocumentsCount > 0)
                    {
                        if (remainingDocumentsCount.ToString().EndsWith("1") &&
                            !remainingDocumentsCount.ToString().EndsWith("11"))
                        {
                            totalDocumentsMessage = String.Format("Осталось обработать {0} документ.",
                                remainingDocumentsCount);
                        }
                        else
                        {
                            totalDocumentsMessage = String.Format("Осталось обработать {0} документов.",
                                remainingDocumentsCount);
                        }

                        ConsoleMessage totalDocumentsConsoleMessage = ConsoleMessageHelper.GetInfoMessage(totalDocumentsMessage);
                        this.ConsoleMessages.Add(totalDocumentsConsoleMessage);
                    }
                    else
                    {
                        totalDocumentsMessage = "Обработка документов завершена.";
                        ConsoleMessage totalDocumentsConsoleMessage = ConsoleMessageHelper.GetSuccessMessage(totalDocumentsMessage);
                        this.ConsoleMessages.Add(totalDocumentsConsoleMessage);
                    }
                }
            });
        }

        private void OnDocumentsProcessingHandler(object sender, EventArgs eventArgs)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                String message = "Обработка документов началась.";
                ConsoleMessage consoleMessage = ConsoleMessageHelper.GetInfoMessage(message);
                this.ConsoleMessages.Add(consoleMessage);
            });
        }

        private void DocumentsProcessedHandler(object sender, EventArgs eventArgs)
        {
            //Application.Current.Dispatcher.Invoke(() =>
            //{
            //    String message = "Обработка документов завершена.";
            //    ConsoleMessage consoleMessage = ConsoleMessageHelper.GetSuccessMessage(message);
            //    this.ConsoleMessages.Add(consoleMessage);
            //});
        }

        #endregion

        #region Helpers

        private void BeginMonitoringCpu()
        {
            Thread thread = new Thread(() =>
            {
                CpuUsage cpuUsage = new CpuUsage();

                try
                {
                    while (true)
                    {
                        Application.Current.Dispatcher.Invoke(() =>
                        {
                            short cpuUsageData = cpuUsage.GetUsage();
                            this.CpuStatistic = new CpuStatistic(cpuUsageData);
                        });

                        Thread.Sleep(250);
                    }
                }
                catch (Exception e)
                {

                }
            });

            thread.Priority = ThreadPriority.Highest;
            thread.IsBackground = true;
            thread.Start();
        }
        
        #endregion
    }
}