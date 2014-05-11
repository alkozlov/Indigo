namespace Indigo.BusinessLogicLayer.Analysis
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public class CompareResult : ReadOnlyCollection<CompareResultSet>
    {
        public LsaResult LsaResult { get; private set; }

        public CompareResult(IList<CompareResultSet> list, LsaResult lsaResult) : base(list)
        {
            this.LsaResult = lsaResult;
        }
    }
}