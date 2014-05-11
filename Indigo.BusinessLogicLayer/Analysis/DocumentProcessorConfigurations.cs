namespace Indigo.BusinessLogicLayer.Analysis
{
    using System;
    using System.Linq;

    using Indigo.BusinessLogicLayer.Shingles;

    public class DocumentProcessorConfigurations
    {
        public String TempApplicationFolder { get; set; }

        public String TempLematizationFilePostfix { get; set; }

        public ShingleSize[] ShihgleSizes { get; set; }

        public DocumentProcessorConfigurations()
        {
            var analysisAccuracyLevels = Enum.GetValues(typeof(ShingleSize)).Cast<ShingleSize>().ToArray();
            this.ShihgleSizes = analysisAccuracyLevels;
        }
    }
}