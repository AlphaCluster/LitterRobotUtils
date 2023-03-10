namespace LitterRobotSplitter.Tests;

public class Tests
{
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void ParseExample()
    {
        CsvParser.ConvertFile(new FileInfo("example.csv"), new FileInfo("output.csv"), 2022, 3);
        Assert.Pass();
    }
}