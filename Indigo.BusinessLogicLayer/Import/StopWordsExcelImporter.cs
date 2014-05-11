namespace Indigo.BusinessLogicLayer.Import
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;

    using Infragistics.Documents.Excel;

    using Indigo.BusinessLogicLayer.Document;

    public class StopWordsExcelImporter : ExcelImporter
    {
        /// <summary>
        /// Read excel file. Ignore first row.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public override async Task<ImportResult> ImportFileToDatabase(String fileName)
        {
            try
            {
                FileInfo fileInfo = new FileInfo(fileName);
                if (!fileInfo.Exists)
                {
                    throw new FileNotFoundException("Can't find file for import.", fileName);
                }
            }
            catch (Exception e)
            {
                throw new ArgumentException("Incorrect file name.", "fileName");
            }

            Workbook dataWorkbook = Workbook.Load(fileName);
            if (dataWorkbook != null && dataWorkbook.Worksheets.Count > 0)
            {
                Worksheet dataWorksheet = dataWorkbook.Worksheets[0];
                List<String> stopWordsForImport = (dataWorksheet.Rows.Where(row => row.Index > 0)
                    .Select(row => row.GetCellText(0))).ToList();

                ImportResult importResult = new ImportResult();
                if (stopWordsForImport.Count > 0)
                {
                    StopWordList stopWordList = await StopWordList.GetAllStopWordsAsync();

                    foreach (var stopWordForImport in stopWordsForImport)
                    {
                        if (!stopWordList.Any(x => x.Content.Equals(stopWordForImport)))
                        {
                            try
                            {
                                StopWord importedStopWord = await StopWord.CreateAsync(stopWordForImport);
                                importResult.RowsInserted++;
                            }
                            catch (DuplicateNameException e)
                            {
                                // We get this exception if stop-word doesn't exists in database but exists two or more times in file
                                importResult.RowsDuplicated++;
                            }
                        }
                        else
                        {
                            importResult.RowsDuplicated++;
                        }
                    }
                }

                return importResult;
            }
            else
            {
                throw new ArgumentNullException("fileName", "Can't load workbook or workbook is empty.");
            }
        }
    }
}