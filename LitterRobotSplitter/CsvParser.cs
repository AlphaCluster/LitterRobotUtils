using System.Globalization;

namespace LitterRobotSplitter;
using Sylvan.Data.Csv;
public class CsvParser
{
    public static void ConvertFile(FileInfo inputFile, FileInfo outputFile, int startingYear, int numberOfCats)
    {
        var currentYear = startingYear;
        using var csv = CsvDataReader.Create(inputFile.FullName);

        var activityIndex = csv.GetOrdinal("Activity");
        var timestampIndex = csv.GetOrdinal("Timestamp");
        var valueIndex = csv.GetOrdinal("Value");

        while(csv.Read()) 
        {
            var activity = csv.GetString(activityIndex);
            var timestampString = csv.GetString(timestampIndex);
            var value = csv.GetString(valueIndex);
            var timestamp = DateTime.ParseExact(timestampString, "M/d h:mmtt", CultureInfo.InvariantCulture);
            timestamp = timestamp.AddYears(currentYear - timestamp.Year);
            var weight = decimal.Parse(value.Substring(0, value.IndexOf(' ')));
            
            Console.WriteLine($"{activity} on {timestamp.ToString()} amount {weight}");
        }
    }
}