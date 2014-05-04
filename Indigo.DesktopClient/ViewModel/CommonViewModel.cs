namespace Indigo.DesktopClient.ViewModel
{
    using System;
    using System.Threading;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Messaging;

    using Indigo.BusinessLogicLayer.Account;
    using Indigo.DesktopClient.Model.Notifications;
    using Indigo.DesktopClient.View;

    public abstract class CommonViewModel : ViewModelBase
    {
        public virtual Boolean IsPartialView
        {
            get { return false; }
        }

        public IndigoUserPrincipal UserPrincipal
        {
            get { return Thread.CurrentPrincipal as IndigoUserPrincipal; }
        }

        //public void NavigateAction(ApplicationView fromView, ApplicationView toView, Object navigationToken = null, Boolean isJumpBack = false)
        //{
        //    NavigationMessage navigationMessage = new NavigationMessage(toView, isJumpBack);
        //    if (navigationToken != null)
        //    {
        //        Messenger.Default.Send<NavigationMessage>(navigationMessage, navigationToken);
        //    }
        //    else
        //    {
        //        Messenger.Default.Send<NavigationMessage>(navigationMessage);
        //    }
        //}

        public void SendNavigationMessage(ApplicationView targetView, NavigationToken token)
        {
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
    }
}
