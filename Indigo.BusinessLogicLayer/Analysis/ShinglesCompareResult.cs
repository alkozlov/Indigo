namespace Indigo.BusinessLogicLayer.Analysis
{
    using Indigo.BusinessLogicLayer.Shingles;

    public class ShinglesCompareResult
    {
        public ShingleSize ShingleSize { get; private set; }

        public float CompareCoefficient { get; private set; }

        public ShinglesCompareResult(ShingleSize shingleSize, float compareCoefficient)
        {
            this.CompareCoefficient = compareCoefficient;
            this.ShingleSize = shingleSize;
        }
    }
}