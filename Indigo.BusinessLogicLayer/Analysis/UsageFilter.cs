namespace Indigo.BusinessLogicLayer.Analysis
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Indigo.BusinessLogicLayer.Document;

    public class UsageFilter
    {
        public static List<DocumentWord> Filter(List<DocumentWord> documentWords, Int32 minimalUsageCount)
        {
            List<DocumentWord> modifiedWords =
                documentWords.Where(
                    documentWord => documentWords.Count(x => x.Word.Equals(documentWord.Word)) >= minimalUsageCount)
                    .ToList();

            return modifiedWords;
        }
    }
}