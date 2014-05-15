namespace Indigo.BusinessLogicLayer.Analysis
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    using Indigo.BusinessLogicLayer.Shingles;

    public class ShinglesResult : ReadOnlyCollection<ShinglesResultSet>
    {
        public ShinglesResult(ShingleSize shingleSize, IList<ShinglesResultSet> list) : base(list)
        {
            this.ShingleSize = shingleSize;
        }

        public ShingleSize ShingleSize { get; private set; }
    }
}