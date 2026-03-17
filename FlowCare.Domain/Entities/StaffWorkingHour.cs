using FlowCare.Domain.Enums;

namespace FlowCare.Domain.Entities;

public sealed class StaffWorkingHour
{
    public long Id { get; init; }
    public int StaffId { get; init; }

    public BranchDayOfWeek DayOfWeek { get; init; }
    public TimeOnly StartTime { get; init; }
    public TimeOnly EndTime { get; init; }

    public Staff? Staff { get; init; }
}
