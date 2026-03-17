using FlowCare.Domain.Enums;

namespace FlowCare.Domain.Entities;

public sealed class Staff
{
    public int Id { get; set; }
    public required string Username { get; init; }
    public required string PasswordHash { get; set; }
    public Role Role { get; init; } = null!;
    public int RoleId { get; init; }


    public required string FullName { get; init; }
    public required string Email { get; init; }
    public int BranchId { get; set; }
    public bool IsActive { get; init; } = true;

    public Branch? Branch { get; init; }

    public ICollection<StaffServiceType> ServiceTypes { get; init; } = new List<StaffServiceType>();
    public ICollection<StaffWorkingHour> WorkingHours { get; init; } = new List<StaffWorkingHour>();
    public ICollection<Slot> Slots { get; init; } = new List<Slot>();
    public ICollection<Appointment> Appointments { get; init; } = new List<Appointment>();
}
