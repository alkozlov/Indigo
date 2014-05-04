namespace Indigo.WinClient.Model.PenthouseModels
{
    using System;
    using System.IO;

    public class NewDocumentModel
    {
        public String DocumentFullName { get; set; }

        public String DocumentName
        {
            get { return Path.GetFileName(this.DocumentFullName); }
        }

        public String DocumentLocation
        {
            get { return Path.GetDirectoryName(this.DocumentFullName); }
        }

        public long Size { get; set; }

        public String SizeAsString
        {
            get
            {
                String sizeAsString = String.Format(new FileSizeFormatProvider(), "{0:fs}", this.Size);
                return sizeAsString;
            }
        }

        public String Thumbnail { get; set; }
    }
}