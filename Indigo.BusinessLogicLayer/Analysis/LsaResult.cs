namespace Indigo.BusinessLogicLayer.Analysis
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public class LsaResultSet
    {
        public Int32? DocumentId { get; set; }

        public float X { get; set; }

        public float Y { get; set; }
    }

    public class LsaResult : ReadOnlyCollection<LsaResultSet>
    {
        public LsaResult(IList<LsaResultSet> list) : base(list)
        {
        }

        public static LsaResult Create(List<LsaResultSet> lsaResultSets)
        {
            LsaResult lsaResult = new LsaResult(lsaResultSets);
            return lsaResult;
        }
    }
}