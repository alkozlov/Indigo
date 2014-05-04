using Indigo.WinClient.View;

namespace Indigo.WinClient.ViewModel
{
    using System;

    using GalaSoft.MvvmLight;
    using GalaSoft.MvvmLight.Messaging;

    using Indigo.WinClient.Model.Notifications;

    public class CommonViewModel : ViewModelBase
    {
        public virtual Boolean IsPartialView
        {
            get { return false; }
        }

        public void SendNavigationMessage(NavigationMessage message, NavigationToken token)
        {
            Messenger.Default.Send(message, token);
        }

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
    }
}