namespace Indigo.Core.Lemmatization
{
    using System;

    public class EmptyFileException : Exception
    {
        public String FileName { get; private set; }

        public EmptyFileException(String message, String fileName)
            : base(message)
        {
            this.FileName = fileName;
        }
    }
}