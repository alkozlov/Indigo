namespace Indigo.DesktopClient.Model.Notifications
{
    using System;

    using GalaSoft.MvvmLight.Messaging;
    using Indigo.DesktopClient.View;

    public class NavigationMessage : NotificationMessage
    {
        public ApplicationView TargetView { get; private set; }

        public NavigationMessage(ApplicationView targetView) : base(String.Empty)
        {
            this.TargetView = targetView;
        }

        public NavigationMessage(string notification, ApplicationView targetView)
            : base(notification)
        {
            this.TargetView = targetView;
        }

        public NavigationMessage(object sender, string notification, ApplicationView targetView)
            : base(sender, notification)
        {
            this.TargetView = targetView;
        }

        public NavigationMessage(object sender, object target, string notification, ApplicationView targetView)
            : base(sender, target, notification)
        {
            this.TargetView = targetView;
        }
    }
}
