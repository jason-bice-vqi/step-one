using System.Data;
using OfficeOpenXml;

namespace ViaQuestInc.StepOne.Infrastructure.Services;

public class ExcelService
{
    public Task<DataSet> ToDataSetAsync(Stream stream, int headerRow, CancellationToken cancellationToken)
    {
        var dataSet = new DataSet();

        stream.Position = 0;

        // Load the spreadsheet
        using var package = new ExcelPackage(stream);

        foreach (var worksheet in package.Workbook.Worksheets)
        {
            // Create a DataTable for each worksheet
            var dataTable = new DataTable(worksheet.Name);

            // Get the worksheet's column headers
            foreach (var cell in worksheet.Cells[headerRow, 1, 1, worksheet.Dimension.End.Column])
            {
                dataTable.Columns.Add(cell.Text);
            }

            // Load worksheet rows into the DataTable
            for (var row = headerRow + 1; row <= worksheet.Dimension.End.Row; row++)
            {
                var dataRow = dataTable.NewRow();
                
                for (var col = 1; col <= worksheet.Dimension.End.Column; col++)
                {
                    dataRow[col - 1] = worksheet.Cells[row, col].Text;
                }
                
                dataTable.Rows.Add(dataRow);
            }

            // Add the DataTable to the DataSet
            dataSet.Tables.Add(dataTable);
        }

        return Task.FromResult(dataSet);
    }
}