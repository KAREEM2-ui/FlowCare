namespace FlowCare.Domain.Entities;

public sealed class Slot
{
    public int Id { get; set; }
    public int BranchId { get; init; }
    public int ServiceTypeId { get; set; }
    public int StaffId { get; set; }

    public DateTimeOffset StartAt { get; set; }
    public DateTimeOffset EndAt { get; set; }

    public int Capacity { get; set; } = 1;
    public bool IsActive { get; set; } = true;
    public DateTimeOffset? DeletedAt { get; set; }

    public Branch? Branch { get; set; }
    public ServiceType? ServiceType { get; init; }
    public Staff? Staff { get; init; }


    public ICollection<Appointment> Appointments { get; init; } = new List<Appointment>();
}
