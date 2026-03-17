public sealed class BranchSlotPosition
{
    public string? FullName { get; init; }
    public DateTimeOffset StartAt { get; init; }
    public DateTimeOffset EndAt { get; init; }
    public string? Service { get; init; }
    public long? RankPosition { get; init; }
}