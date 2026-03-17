using System.Collections.Generic;

namespace FlowCare.Domain.Entities;

public sealed class Branch
{
    public int Id { get; set; }
    public required string Name { get; init; }
    public required string City { get; init; }
    public required string Address { get; init; }
    public required string Timezone { get; init; }
    public bool IsActive { get; init; } = true;
    public int RetentionPerioud { get; set; }

    public ICollection<BranchWorkingHour> WorkingHours { get; init; } = new List<BranchWorkingHour>();
    public ICollection<ServiceType> ServiceTypes { get; init; } = new List<ServiceType>();
    public ICollection<Slot> Slots { get; init; } = new List<Slot>();
}
