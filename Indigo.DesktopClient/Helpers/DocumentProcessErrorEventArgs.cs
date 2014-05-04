﻿namespace Indigo.DesktopClient.Helpers
{
    using System;

    public class DocumentProcessErrorEventArgs : EventArgs
    {
        public String DocumentName { get; private set; }

        public Exception Exception { get; private set; }

        public DocumentProcessErrorEventArgs(String documentName, Exception processException)
        {
            this.DocumentName = documentName;
            this.Exception = processException;
        }
    }
}