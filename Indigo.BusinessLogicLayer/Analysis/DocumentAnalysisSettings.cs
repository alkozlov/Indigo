namespace Indigo.BusinessLogicLayer.Analysis
{
    using System;

    using Indigo.BusinessLogicLayer.Shingles;

    public class DocumentAnalysisSettings
    {
        public DocumentAnalysisSettings(ShingleSize shingleSize, float minimalSimilarityLevel)
        {
            this.ShingleSize = shingleSize;
            this.MinimalSimilarityLevel = minimalSimilarityLevel;
        }

        public ShingleSize ShingleSize { get; private set; }

        public float MinimalSimilarityLevel { get; private set; }
    }
}