using System;

namespace Indigo.BusinessLogicLayer.Analysis
{
    public class DocumentProcessedEventArgs
    {
        public String DocumentName { get; private set; }

        public DocumentProcessedEventArgs(String documentName)
        {
            this.DocumentName = documentName;
        }
    }
}