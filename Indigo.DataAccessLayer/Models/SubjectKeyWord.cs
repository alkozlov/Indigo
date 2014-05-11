namespace Indigo.DataAccessLayer.Models
{
    using System;

    public class SubjectKeyWord
    {
        public Int32 SubjectKeyWordId { get; set; }

        public String Word { get; set; }

        public Int32 SubjectId { get; set; }

        public Subject Subject { get; set; }
    }
}