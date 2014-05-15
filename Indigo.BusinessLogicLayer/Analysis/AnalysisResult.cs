namespace Indigo.BusinessLogicLayer.Analysis
{
    using System;
    using System.Collections.Generic;

    using Indigo.BusinessLogicLayer.Document;

    public class AnalysisResult
    {
        public DocumentKeyWordList DocumentKeyWords { get; set; }

        public AnalysisResult()
        {
            this.DocumentKeyWords = null;
        }
    }
}