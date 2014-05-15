using System;
using System.Threading.Tasks;

namespace Indigo.BusinessLogicLayer.Reports
{
    public class ReportGenerator
    {
        #region Singltone

        private static ReportGenerator _current;

        public static ReportGenerator Current
        {
            get { return _current ?? (_current = new ReportGenerator()); }
        }

        #endregion

        #region Constructors

        private ReportGenerator()
        {
        }

        #endregion

        public async Task GenerateReportAsync(IReport report, Boolean isOpenAfterGeneration)
        {
            
        }
    }
}