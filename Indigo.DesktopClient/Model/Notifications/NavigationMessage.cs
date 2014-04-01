namespace Indigo.DesktopClient.Model.Notifications
{
    using System;

    using GalaSoft.MvvmLight.Messaging;
    using Indigo.DesktopClient.View;

    public class NavigationMessage : NotificationMessage
    {
        public ApplicationView TargetView { get; private set; }

        public Boolean IsBackJump { get; private set; }

        public NavigationMessage(ApplicationView targetView, Boolean isBackJump) : base(String.Empty)
        {
            this.IsBackJump = isBackJump;
            this.TargetView = targetView;
        }

        public NavigationMessage(string notification, ApplicationView targetView, Boolean isBackJump)
            : base(notification)
        {
            this.IsBackJump = isBackJump;
            this.TargetView = targetView;
        }

        public NavigationMessage(object sender, string notification, ApplicationView targetView, Boolean isBackJump)
            : base(sender, notification)
        {
            this.IsBackJump = isBackJump;
            this.TargetView = targetView;
        }

        public NavigationMessage(object sender, object target, string notification, ApplicationView targetView, Boolean isBackJump)
            : base(sender, target, notification)
        {
            this.IsBackJump = isBackJump;
            this.TargetView = targetView;
        }
    }
}
