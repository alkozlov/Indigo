namespace Indigo.DesktopClient.Model.Notifications
{
    using System;

    using GalaSoft.MvvmLight.Messaging;

    using Indigo.BusinessLogicLayer.Analysis;

    public class SimilarDocumentsSearchNotification : NotificationMessage
    {
        public ShinglesResultSet ShinglesResultSet { get; private set; }

        public SimilarDocumentsSearchNotification(ShinglesResultSet shinglesResultSet)
            : base(String.Empty)
        {
            this.ShinglesResultSet = shinglesResultSet;
        }

        private SimilarDocumentsSearchNotification(string notification, ShinglesResultSet shinglesResultSet)
            : base(notification)
        {
            this.ShinglesResultSet = shinglesResultSet;
        }

        private SimilarDocumentsSearchNotification(object sender, string notification, ShinglesResultSet shinglesResultSet)
            : base(sender, notification)
        {
            this.ShinglesResultSet = shinglesResultSet;
        }

        private SimilarDocumentsSearchNotification(object sender, object target, string notification, ShinglesResultSet shinglesResultSet)
            : base(sender, target, notification)
        {
            this.ShinglesResultSet = shinglesResultSet;
        }
    }
}