namespace Indigo.BusinessLogicLayer.Analysis
{
    public class CompareResult
    {
        public CompareResult()
        {
            this.ShinglesResult = null;
            this.LsaResult = null;
        }

        public CompareResult(ShinglesResult shinglesResult, LsaResult lsaResult)
        {
            this.ShinglesResult = shinglesResult;
            this.LsaResult = lsaResult;
        }

        public ShinglesResult ShinglesResult { get; private set; }

        public LsaResult LsaResult { get; private set; }
    }
}