namespace Indigo.BusinessLogicLayer.Analysis
{
    using System;
    using System.Collections.Generic;

    using Indigo.BusinessLogicLayer.Shingles;

    public class ShinglesResultSet
    {
        public ShinglesResultSet(Int32 documentId, float matchingCoefficient, List<Shingle> similarShingles)
        {
            this.DocumentId = documentId;
            this.MatchingCoefficient = matchingCoefficient;
            this.SimilarShingles = similarShingles;
        }

        public Int32 DocumentId { get; private set; }

        public float MatchingCoefficient { get; private set; }

        public List<Shingle> SimilarShingles { get; private set; }
    }
}