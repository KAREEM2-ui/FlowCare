namespace FlowCare.Domain.Entities;

public sealed class ServiceType
{
    public int Id { get; set; }
    public int BranchId { get; init; }
    public required string Name { get; init; }
    public string? Description { get; init; }
    public int DurationMinutes { get; init; }
    public bool IsActive { get; init; } = true;

    public Branch? Branch { get; init; }

    public ICollection<StaffServiceType> Staff { get; init; } = new List<StaffServiceType>();
    public ICollection<Slot> Slots { get; init; } = new List<Slot>();
}
