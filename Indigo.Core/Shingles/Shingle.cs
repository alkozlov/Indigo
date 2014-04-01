namespace Indigo.Core.Shingles
{
    using System;
    using System.Collections.Generic;

    public class Shingle
    {
        public List<String> Words { get; private set; }

        public Shingle()
        {
            this.Words = new List<String>();
        }

        public Shingle(List<String> words)
        {
            this.Words = words;
        }
    }
}
