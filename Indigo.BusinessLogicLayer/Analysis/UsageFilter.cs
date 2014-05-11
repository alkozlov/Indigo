namespace Indigo.BusinessLogicLayer.Analysis
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class UsageFilter
    {
        public static List<String> Filter(List<String> documentWords, Int32 minimalUsageCount)
        {
            List<String> modifiedWords =
                documentWords.Where(documentWord => documentWords.Count(x => x.Equals(documentWord)) >= minimalUsageCount).ToList();

            return modifiedWords;
        }
    }
}