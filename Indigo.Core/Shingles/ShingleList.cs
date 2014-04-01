namespace Indigo.Core.Shingles
{
    using System;
    using System.Collections.Generic;
    using System.Collections.ObjectModel;

    public class ShingleList : ReadOnlyCollection<Shingle>
    {
        public Int32 ShingleSize { get; private set; }

        public ShingleList(IList<Shingle> list, Int32 shingleSize) : base(list)
        {
            this.ShingleSize = shingleSize;
        }
    }
}