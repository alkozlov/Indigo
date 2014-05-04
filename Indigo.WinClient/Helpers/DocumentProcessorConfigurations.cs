using System;
using System.Linq;
using Indigo.BusinessLogicLayer.Shingles;

namespace Indigo.WinClient.Helpers
{
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