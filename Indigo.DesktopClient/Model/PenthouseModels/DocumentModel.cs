namespace Indigo.DesktopClient.Model.PenthouseModels
{
    using System;
    using System.Collections.Generic;

    public class DocumentModel
    {
        public Guid DocumentGuid { get; set; }

        public String DocumentFileName { get; set; }

        public String StorageFileName { get; set; }

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

                if (this.DocumentFileName.ToLower().EndsWith(".doc"))
                {
                    documentType = DocumentType.Doc;
                }
                else if (this.DocumentFileName.ToLower().EndsWith(".docx"))
                {
                    documentType = DocumentType.Docx;
                }
                else if (this.DocumentFileName.ToLower().EndsWith(".odt"))
                {
                    documentType = DocumentType.Odt;
                }
                else if (this.DocumentFileName.ToLower().EndsWith(".rtf"))
                {
                    documentType = DocumentType.Rtf;
                }

                return documentType;
            }
        }

        public Dictionary<Int32, Int32> ShinglesStatistic { get; set; }

        public String AddedByUser { get; set; }

        public Boolean IsCorrupted { get; set; }

        public DocumentModel()
        {
            this.ShinglesStatistic = new Dictionary<Int32, Int32>();
        }
    }
}