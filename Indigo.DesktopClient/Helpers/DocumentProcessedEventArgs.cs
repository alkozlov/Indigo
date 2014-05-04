namespace Indigo.DesktopClient.Helpers
{
    using System;

    public class DocumentProcessedEventArgs
    {
        public String DocumentName { get; private set; }

        public DocumentProcessedEventArgs(String documentName)
        {
            this.DocumentName = documentName;
        }
    }
}