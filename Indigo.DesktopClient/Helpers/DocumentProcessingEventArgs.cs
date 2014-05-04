namespace Indigo.DesktopClient.Helpers
{
    using System;

    public class DocumentProcessingEventArgs : EventArgs
    {
        public String DocumentName { get; private set; }

        public DocumentProcessingEventArgs(String documentName)
        {
            this.DocumentName = documentName;
        }
    }
}