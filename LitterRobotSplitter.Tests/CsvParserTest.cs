namespace LitterRobotSplitter.Tests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void FullPathTest()
    {
        var weights = CsvParser.ParseFile(new FileInfo("example.csv"), 2023);
        var splitList = CsvParser.SplitForCats(weights, 3);
        var writer = new ExcelSplitWriter();
        writer.WriteToFile(new FileInfo("test.xlsx"), splitList);
        
        foreach (var weightList in splitList)
        {
            Console.WriteLine("--- NEW CAT ---");
            foreach (var entry in weightList)
                Console.WriteLine($"{entry.Timestamp.ToString()} amount {entry.Weight}");
        }
        Assert.Pass();
    }
}