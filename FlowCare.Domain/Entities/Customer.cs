using FlowCare.Domain.Enums;

namespace FlowCare.Domain.Entities;

public sealed class Customer
{
    public int Id { get; set; }
    public required string Username { get; init; }
    public required string PasswordHash { get; set; }
    public required string FullName { get; init; }
    public required string Email { get; init; }
    public string? Phone { get; init; }
    public bool IsActive { get; init; } = true;
    public string? IdRefImage { get; init; }

    public ICollection<Appointment> Appointments { get; init; } = new List<Appointment>();
}
