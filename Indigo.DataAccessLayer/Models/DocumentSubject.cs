namespace Indigo.DataAccessLayer.Models
{
    using System;

    public class DocumentSubject
    {
        public Int32 DocumentId { get; set; }

        public Int32 SubjectId { get; set; }

        public Document Document { get; set; }

        public Subject Subject { get; set; }
    }
}