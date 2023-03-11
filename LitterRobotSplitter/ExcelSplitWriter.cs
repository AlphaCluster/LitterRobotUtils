using System.Data.Common;
using ClosedXML.Excel;
using DocumentFormat.OpenXml.Spreadsheet;

namespace LitterRobotSplitter;

public class ExcelSplitWriter : ISplitWriter
{
    public void WriteToFile(FileInfo filePath, IList<IList<WeightEntry>> listOfWeights)
    {
        using var workbook = new XLWorkbook();
        foreach (var weights in listOfWeights)
        {
            var worksheet = workbook.Worksheets.Add();
            var currentRow = 1;
            foreach (var weight in weights)
            {
                worksheet.Cell(currentRow, 1).Value = weight.Timestamp;
                worksheet.Cell(currentRow, 2).Value = weight.Weight;
                currentRow++;
            }
        }
        
        workbook.SaveAs(filePath.FullName);
    }
}