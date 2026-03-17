using FlowCare.Domain.Enums;

namespace FlowCare.Domain.Entities;

public sealed class BranchWorkingHour
{
    public long Id { get; init; }
    public required int BranchId { get; init; }
    public BranchDayOfWeek DayOfWeek { get; init; }
    public TimeOnly StartTime { get; init; }
    public TimeOnly EndTime { get; init; }

    public Branch? Branch { get; init; }
}
