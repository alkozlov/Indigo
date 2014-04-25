namespace Indigo.DataAccessLayer.Models
{
    using System;

    public class Shingle
    {
        public long ShingleId { get; set; }

        public Int32 DocumentId { get; set; }

        public byte ShingleSize { get; set; }

        public long CheckSum { get; set; }

        public Document Document { get; set; }
    }
}