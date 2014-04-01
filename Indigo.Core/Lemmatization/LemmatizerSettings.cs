namespace Indigo.Core.Lemmatization
{
    public class LemmatizerSettings
    {
        public WordWrappingMode WordWrappingMode { get; set; }

        public WordformsOutputMode WordformsOutputMode { get; set; }

        public GrammaticalInformationOutputMode GrammaticalInformationOutputMode { get; set; }

        public DocumentPunctuationMode DocumentPunctuationMode { get; set; } 
    }
}