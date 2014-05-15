namespace Indigo.BusinessLogicLayer.Document
{
    using System;

    public class DocumentWord
    {
        public String Word { get; set; }

        public Int32 StartIndex { get; set; }

        public Int32 EndIndex
        {
            get { return this.StartIndex + this.Word.Length; }
        }
    }
}