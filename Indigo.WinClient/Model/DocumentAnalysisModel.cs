namespace Indigo.WinClient.Model
{
    using System;

    public class DocumentAnalysisModel
    {
        public String FullName { get; set; }

        public String FileName { get; set; }

        public long Size { get; set; }

        public String SizeAsString
        {
            get
            {
                String sizeAsString = String.Format(new FileSizeFormatProvider(), "{0:fs}", this.Size);
                return sizeAsString;
            }
        }

        public String Directory { get; set; }

        public String Thumbnail { get; set; }

        public String TempFileFullName { get; set; }
    }
}