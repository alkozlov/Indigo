namespace Indigo.BusinessLogicLayer.Reports
{
    using System;
    using System.Threading.Tasks;

    public interface IReport
    {
        String ReportFileName { get; }

        Task GenerateAsync();
    }
}