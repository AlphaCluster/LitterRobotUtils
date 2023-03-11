namespace LitterRobotSplitter;

public record WeightEntry
{
    public required DateTime Timestamp { get; set; }
    public required decimal Weight { get; set; }
}