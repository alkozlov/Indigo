namespace Indigo.DataAccessLayer.Models
{
    using System;

    public class DocumentKeyWord
    {
        public Int64 DocumentKeyWordId { get; set; }

        public String Word { get; set; }

        public Int32 Usages { get; set; }

        public Int32 DocumentId { get; set; }

        public Document Document { get; set; }
    }
}