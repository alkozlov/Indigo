namespace Indigo.BusinessLogicLayer.Import
{
    using System;

    public class ImportResult
    {
        public Int32 RowsInserted { get; set; }

        public Int32 RowsDuplicated { get; set; }

        public ImportResult()
        {
            this.RowsInserted = 0;
            this.RowsDuplicated = 0;
        }
    }
}