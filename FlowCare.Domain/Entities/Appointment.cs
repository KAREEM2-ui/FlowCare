using FlowCare.Domain.Enums;

namespace FlowCare.Domain.Entities;

public sealed class Appointment
{
    public int Id { get; set; }

    public int CustomerId { get; init; }
    public int BranchId { get; init; }
    public int ServiceTypeId { get; init; }
    public int SlotId { get; set; }
    public int StaffId { get; init; }

    public AppointmentStatus Status { get; set; } = AppointmentStatus.Booked;
    public DateTimeOffset CreatedAt { get; init; }

    public Customer? Customer { get; init; }
    public Branch? Branch { get; init; }
    public ServiceType? ServiceType { get; init; }
    public Slot? Slot { get; init; }
    public Staff? Staff { get; init; }

    public string? AttachementReference { get; set; }
}
