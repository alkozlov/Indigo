namespace Indigo.DesktopClient.Model.Notifications
{
    using System;

    using GalaSoft.MvvmLight.Messaging;
    using Indigo.DesktopClient.View;

    public class AuthorizationMessage : NotificationMessage
    {
        public Boolean IsAuthorized { get; private set; }

        public AuthorizationMessage(String notification, Boolean isAuthorized) : base(notification)
        {
            this.IsAuthorized = isAuthorized;
        }

        public AuthorizationMessage(object sender, String notification, Boolean isAuthorized) : base(sender, notification)
        {
            this.IsAuthorized = isAuthorized;
        }

        public AuthorizationMessage(object sender, object target, String notification, Boolean isAuthorized) : base(sender, target, notification)
        {
            this.IsAuthorized = isAuthorized;
        }
    }
}