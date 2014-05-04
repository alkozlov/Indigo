namespace Indigo.DesktopClient.Model.PenthouseModels
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

        public DocumentType DocumentType
        {
            get
            {
                DocumentType documentType = DocumentType.Doc;

                if (this.DocumentFullName.ToLower().EndsWith(".doc"))
                {
                    documentType = DocumentType.Doc;
                }
                else if (this.DocumentFullName.ToLower().EndsWith(".docx"))
                {
                    documentType = DocumentType.Docx;
                }
                else if (this.DocumentFullName.ToLower().EndsWith(".odt"))
                {
                    documentType = DocumentType.Odt;
                }
                else if (this.DocumentFullName.ToLower().EndsWith(".rtf"))
                {
                    documentType = DocumentType.Rtf;
                }

                return documentType;
            }
        }

        public ProcessingStatus ProcessingStatus { get; set; }
    }
}