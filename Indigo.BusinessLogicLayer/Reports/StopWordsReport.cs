using System.Threading.Tasks;
using Indigo.BusinessLogicLayer.Document;
using Infragistics.Documents.Excel;

namespace Indigo.BusinessLogicLayer.Reports
{
    using System;

    public class StopWordsReport : IReport
    {
        public StopWordsReport(String reportFileName)
        {
            this.ReportFileName = reportFileName;
        }

        public String ReportFileName { get; private set; }
        public async Task GenerateAsync()
        {
            StopWordList stopWordList = await StopWordList.GetAllStopWordsAsync();
            Workbook workbook = new Workbook(WorkbookFormat.Excel2007);
            Worksheet sheetOne = workbook.Worksheets.Add("Стоп-слова");

            this.SetCellValue(sheetOne.Rows[0].Cells[0], "ID");
            this.SetCellValue(sheetOne.Rows[0].Cells[2], "Стоп-слово");

            Int32 currentRow = 1;
            WorksheetRow worksheetRow;
            foreach (var stopWord in stopWordList)
            {
                Int32 currentCell = 0;
                worksheetRow = sheetOne.Rows[currentRow];

                this.SetCellValue(worksheetRow.Cells[currentCell], stopWord.StopWordId);
                this.SetCellValue(worksheetRow.Cells[++currentCell], stopWord.Content);

                currentRow++;
            }

            this.SetColumnFormatting(sheetOne);
            workbook.Save(this.ReportFileName);
        }

        private void SetCellValue(WorksheetCell cell, object value)
        {
            cell.Value = value;
            cell.CellFormat.ShrinkToFit = ExcelDefaultableBoolean.True;
            cell.CellFormat.VerticalAlignment = VerticalCellAlignment.Center;
            cell.CellFormat.Alignment = HorizontalCellAlignment.Center;
        }

        private void SetColumnFormatting(Worksheet worksheet)
        {
            // Freeze header row
            worksheet.DisplayOptions.PanesAreFrozen = true;
            worksheet.DisplayOptions.FrozenPaneSettings.FrozenRows = 1;

            // Build Column List
            worksheet.DefaultColumnWidth = 5000;
            worksheet.Columns[1].Width = 8500;
            worksheet.Columns[3].Width = 10000;
        }
    }
}