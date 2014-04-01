namespace Indigo.DataAccessLayer.Models
{
    using System;

    public class Document
    {
        public Int32 DocumentId { get; set; }

        public Guid DocumentGuid { get; set; }

        public String FileExtension { get; set; }

        public String OriginalName { get; set; }

        public String StoredName { get; set; }

        public String LematizationName { get; set; }

        public Int32 AddedUserId { get; set; }

        public DateTime CreateDateTimeUtc { get; set; }


    }
}
