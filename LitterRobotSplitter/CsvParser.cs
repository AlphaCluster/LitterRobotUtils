using System.Globalization;

namespace LitterRobotSplitter;
using Sylvan.Data.Csv;

public record WeightEntry
{
    public required DateTime Timestamp { get; set; }
    public required decimal Weight { get; set; }
}

public class CsvParser
{
    public static IList<WeightEntry> ParseFile(FileInfo inputFile, int startingYear)
    {
        IList<WeightEntry> weights = new List<WeightEntry>();
        var currentYear = startingYear;
        var lastMonth = 0;
        using var csv = CsvDataReader.Create(inputFile.FullName);

        var activityIndex = csv.GetOrdinal("Activity");
        var timestampIndex = csv.GetOrdinal("Timestamp");
        var valueIndex = csv.GetOrdinal("Value");

        while(csv.Read()) 
        {
            var activity = csv.GetString(activityIndex);
            var timestampString = csv.GetString(timestampIndex);
            var value = csv.GetString(valueIndex);
            
            // Parse Date
            var timestamp = DateTime.ParseExact(timestampString, "M/d h:mmtt", CultureInfo.InvariantCulture);
            if (lastMonth > timestamp.Month)
                currentYear++;
            lastMonth = timestamp.Month;
            timestamp = timestamp.AddYears(currentYear - timestamp.Year);
            
            var weight = decimal.Parse(value.Substring(0, value.IndexOf(' ')));
            
            weights.Add(new WeightEntry
            {
                Timestamp = timestamp,
                Weight = weight
            });
        }

        return weights;
    }


    public static List<WeightEntry>[] SplitForCats(IList<WeightEntry> weights, int numberOfCats)
    {
        const int DRIFT = 1;
        var catLastWeight = new decimal[numberOfCats];
        for (int index = 0; index < numberOfCats; index++)
            catLastWeight[index] = 0;
        List<WeightEntry>[] weightLists = new List<WeightEntry>[numberOfCats];
        for (int index = 0; index < numberOfCats; index++)
            weightLists[index] = new List<WeightEntry>();

        foreach (var entry in weights)
        {
            var currentCat = -1;
            for (int index = 0; index < numberOfCats; index++)
            {
                if ((catLastWeight[index] == 0) ||
                    (Math.Abs(catLastWeight[index] - entry.Weight) < DRIFT))
                {
                    currentCat = index;
                    catLastWeight[index] = entry.Weight;
                    weightLists[index].Add(new WeightEntry
                    {
                        Timestamp = entry.Timestamp,
                        Weight = entry.Weight
                    });
                    break;
                }
            }

            if (currentCat < 0)
                Console.WriteLine("skipped a cat");
        }

        return weightLists;
    }
}