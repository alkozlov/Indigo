namespace Indigo.BusinessLogicLayer.Import
{
    using System;
    using System.Threading.Tasks;

    public abstract class ExcelImporter
    {
        public abstract Task<ImportResult> ImportFileToDatabase(String fileName);
    }
}