using System;

namespace Indigo.BusinessLogicLayer.Analysis
{
    public class DocumentProcessingEventArgs : EventArgs
    {
        public String DocumentName { get; private set; }

        public DocumentProcessingEventArgs(String documentName)
        {
            this.DocumentName = documentName;
        }
    }
}