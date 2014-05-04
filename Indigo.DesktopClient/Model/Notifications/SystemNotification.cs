namespace Indigo.DesktopClient.Model.Notifications
{
    using System;

    public enum NotificationType : byte
    {
        Empty = 0,

        Error = 1,

        Success = 2,

        Info = 3,

        Warning = 4
    }

    public class SystemNotification
    {
        public String Message { get; set; }

        public NotificationType Type { get; set; }
    }
}