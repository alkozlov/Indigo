using System.Linq;

namespace Indigo.DesktopClient.Helpers
{
    using Indigo.BusinessLogicLayer.Shingles;

    using System;

    public class DocumentProcessorConfigurations
    {
        public String TempApplicationFolder { get; set; }

        public String TempLematizationFilePostfix { get; set; }

        public byte[] ShihgleSizes { get; set; }

        public DocumentProcessorConfigurations()
        {
            var analysisAccuracyLevels = Enum.GetValues(typeof(AnalysisAccuracy)).Cast<byte>().ToArray();
            this.ShihgleSizes = analysisAccuracyLevels;
        }
    }
}