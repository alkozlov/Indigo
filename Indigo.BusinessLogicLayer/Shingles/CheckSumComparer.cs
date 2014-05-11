namespace Indigo.BusinessLogicLayer.Shingles
{
    using System;
    using System.Linq;

    public class CheckSumComparer
    {
        public static float CalculateMatchingPercentage(CheckSumCollection checkSumCollection1, CheckSumCollection checkSumCollection2)
        {
            Int32 matchCount = checkSumCollection1.Count(checkSum => checkSumCollection2.Any(x => x.Key.Equals(checkSum.Key)));

            double matchingPercentage = Math.Round((double)matchCount/(double)checkSumCollection1.Count, 2);
            float matchingPercentageResult = Convert.ToSingle(matchingPercentage);
            return matchingPercentageResult;
        }
    }
}