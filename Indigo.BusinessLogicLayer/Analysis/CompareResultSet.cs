namespace Indigo.BusinessLogicLayer.Analysis
{
    using System;

    public class CompareResultSet
    {
        public Int32 DocumentId { get; private set; }

        public ShinglesCompareResult ShinglesCompareResult { get; private set; }

        public CompareResultSet(Int32 documentId, ShinglesCompareResult shinglesCompareResult)
        {
            this.DocumentId = documentId;
            this.ShinglesCompareResult = shinglesCompareResult;
        }
    }
}