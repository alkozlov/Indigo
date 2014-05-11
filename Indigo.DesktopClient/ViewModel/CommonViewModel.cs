using Indigo.DesktopClient.Model.DocumentAnalysis;

namespace Indigo.DesktopClient.ViewModel
{
    using System;
    using System.Threading;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Ioc;
    using GalaSoft.MvvmLight.Messaging;

    using Indigo.BusinessLogicLayer.Account;
    using Indigo.BusinessLogicLayer.Analysis;
    using Indigo.BusinessLogicLayer.Shingles;
    using Indigo.DesktopClient.Model.Notifications;
    using Indigo.DesktopClient.View;
    using Indigo.DesktopClient.ViewModel.Partial;

    public abstract class CommonViewModel : ViewModelBase
    {
        #region Defaults values

        private const ShingleSize DefaultShingleSize = ShingleSize.Size7;
        private const Int32 DefaultMinimalSimilarityLevel = 10;

        #endregion

        public virtual Boolean IsPartialView
        {
            get { return false; }
        }

        public IndigoUserPrincipal UserPrincipal
        {
            get { return Thread.CurrentPrincipal as IndigoUserPrincipal; }
        }

        public void SendNavigationMessage(ApplicationView targetView, NavigationToken token)
        {
            // Reset target view model
            this.ResetViewModel(targetView);

            NavigationMessage message = new NavigationMessage(targetView);
            Messenger.Default.Send(message, token);
        }

        public void SigninMessageSend()
        {
            AuthorizationMessage message = new AuthorizationMessage("Signin success!", true);
            Messenger.Default.Send<AuthorizationMessage>(message, NavigationToken.AuthorizationPanelToken);
        }

        public void SignoutMessageSend()
        {
            AuthorizationMessage message = new AuthorizationMessage("Signout success!", false);
            Messenger.Default.Send<AuthorizationMessage>(message, NavigationToken.AuthorizationPanelToken);
        }

        public SystemNotification GetSuccessNotification(String message)
        {
            SystemNotification systemNotification = new SystemNotification
            {
                Message = message,
                Type = NotificationType.Success
            };

            return systemNotification;
        }

        public SystemNotification GetErrorNotification(String message)
        {
            SystemNotification systemNotification = new SystemNotification
            {
                Message = message,
                Type = NotificationType.Error
            };

            return systemNotification;
        }

        public SystemNotification GetWarningNotification(String message)
        {
            SystemNotification systemNotification = new SystemNotification
            {
                Message = message,
                Type = NotificationType.Warning
            };

            return systemNotification;
        }

        public SystemNotification GetInfoNotification(String message)
        {
            SystemNotification systemNotification = new SystemNotification
            {
                Message = message,
                Type = NotificationType.Info
            };

            return systemNotification;
        }

        public SystemNotification GetNotification(String message, NotificationType notificationType)
        {
            SystemNotification systemNotification = new SystemNotification
            {
                Message = message,
                Type = notificationType
            };

            return systemNotification;
        }

        public void ResetViewModel(ApplicationView targetView)
        {
            switch (targetView)
            {
                case ApplicationView.AddDocuments:
                {
                    SimpleIoc.Default.Unregister<AddDocumentsViewModel>();
                    SimpleIoc.Default.Register<AddDocumentsViewModel>();
                } break;

                case ApplicationView.Analysis:
                {
                    SimpleIoc.Default.Unregister<AnalysisViewModel>();
                    SimpleIoc.Default.Register<AnalysisViewModel>();
                } break;

                case ApplicationView.AnalysisResults:
                {
                    // TODO:
                } break;

                case ApplicationView.AnalysisSettings:
                {
                    // TODO:
                } break;

                case ApplicationView.AuthorizedCommandPanel:
                {
                    SimpleIoc.Default.Unregister<AuthorizedViewModel>();
                    SimpleIoc.Default.Register<AuthorizedViewModel>();
                } break;

                case ApplicationView.DocumentAnalysis:
                {
                    SimpleIoc.Default.Unregister<DocumentAnalysisViewModel>();
                    SimpleIoc.Default.Register<DocumentAnalysisViewModel>();
                } break;

                case ApplicationView.DocumentsDatabase:
                {
                    SimpleIoc.Default.Unregister<DocumentsViewModel>();
                    SimpleIoc.Default.Register<DocumentsViewModel>();
                } break;

                case ApplicationView.HomaPage:
                {
                    // TODO: а нужно ли?
                } break;

                case ApplicationView.Main:
                {
                    // TODO: а нужно ли?
                } break;

                case ApplicationView.PasswordRecovery:
                {
                    // TODO:
                } break;

                case ApplicationView.Penthouse:
                {
                    SimpleIoc.Default.Unregister<PenthouseViewModel>();
                    SimpleIoc.Default.Register<PenthouseViewModel>();
                } break;

                case ApplicationView.Profile:
                {
                    SimpleIoc.Default.Unregister<ProfileViewModel>();
                    SimpleIoc.Default.Register<ProfileViewModel>();
                } break;

                case ApplicationView.References:
                {
                    SimpleIoc.Default.Unregister<ReferencesViewModel>();
                    SimpleIoc.Default.Register<ReferencesViewModel>();
                } break;

                case ApplicationView.Reports:
                {
                    SimpleIoc.Default.Unregister<ReportsViewModel>();
                    SimpleIoc.Default.Register<ReportsViewModel>();
                } break;

                case ApplicationView.SignIn:
                {
                    SimpleIoc.Default.Unregister<SignInViewModel>();
                    SimpleIoc.Default.Register<SignInViewModel>();
                } break;

                case ApplicationView.SignUp:
                {
                    // TODO:
                } break;

                case ApplicationView.StopWords:
                {
                    SimpleIoc.Default.Unregister<StopWordsViewModel>();
                    SimpleIoc.Default.Register<StopWordsViewModel>();
                } break;

                case ApplicationView.Subjects:
                {
                    SimpleIoc.Default.Unregister<SubjectsViewModel>();
                    SimpleIoc.Default.Register<SubjectsViewModel>();
                } break;

                case ApplicationView.TextAnalisys:
                {
                    SimpleIoc.Default.Unregister<TextAnalysisViewModel>();
                    SimpleIoc.Default.Register<TextAnalysisViewModel>();
                } break;

                case ApplicationView.UnauthorizedCommandPanel:
                {
                    SimpleIoc.Default.Unregister<UnauthorizedViewModel>();
                    SimpleIoc.Default.Register<UnauthorizedViewModel>();
                } break;

                case ApplicationView.UsersDatabase:
                {
                    SimpleIoc.Default.Unregister<UsersViewModel>();
                    SimpleIoc.Default.Register<UsersViewModel>();
                } break;

                case ApplicationView.AddDocument:
                {
                    SimpleIoc.Default.Unregister<AddDocumentViewModel>();
                    SimpleIoc.Default.Register<AddDocumentViewModel>();
                } break;
            }
        }

        public AnalysisSettings GetDefaultShingleSize()
        {
            AnalysisSettings defaultAnalysisSettings = new AnalysisSettings
            {
                ShingleSize = (byte) DefaultShingleSize,
                MinimalSimilarityLevel = DefaultMinimalSimilarityLevel
            };

            return defaultAnalysisSettings;
        }
    }
}
