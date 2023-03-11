namespace LitterRobotSplitter;

public interface ISplitWriter
{
    public void WriteToFile(FileInfo filePath, IList<IList<WeightEntry>> ListOfWeights);
}