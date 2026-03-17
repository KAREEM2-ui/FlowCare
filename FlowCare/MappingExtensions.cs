using FlowCare.Application.DTOs.Responses;
using FlowCare.Domain.Entities;

namespace FlowCare_presentation;

public static class MappingExtensions
{
    public static BranchResponse ToResponse(this Branch b)
        => new(b.Id, b.Name, b.City, b.Address, b.Timezone, b.IsActive);

    public static List<BranchResponse> ToResponse(this List<Branch> list)
        => list.Select(b => b.ToResponse()).ToList();

    public static ServiceTypeResponse ToResponse(this ServiceType s)
        => new(s.Id, s.BranchId, s.Name, s.Description, s.DurationMinutes, s.IsActive);

    public static List<ServiceTypeResponse> ToResponse(this List<ServiceType> list)
        => list.Select(s => s.ToResponse()).ToList();

    public static SlotResponse ToResponse(this Slot s)
        => new(s.Id, s.BranchId, s.ServiceTypeId, s.StaffId,
            s.StartAt, s.EndAt, s.Capacity, s.IsActive, s.Staff?.FullName);

    public static List<SlotResponse> ToResponse(this List<Slot> list)
        => list.Select(s => s.ToResponse()).ToList();

    public static StaffResponse ToResponse(this Staff s)
        => new(s.Id, s.Username, s.FullName, s.Email, s.BranchId,
            s.Branch?.Name, s.Role?.Title, s.IsActive);

    public static List<StaffResponse> ToResponse(this List<Staff> list)
        => list.Select(s => s.ToResponse()).ToList();

    public static CustomerResponse ToResponse(this Customer c)
        => new(c.Id, c.Username, c.FullName, c.Email, c.Phone, c.IsActive);

    public static List<CustomerResponse> ToResponse(this List<Customer> list)
        => list.Select(c => c.ToResponse()).ToList();

    public static AppointmentResponse ToResponse(this Appointment a)
        => new(a.Id, a.CustomerId, a.BranchId, a.ServiceTypeId, a.SlotId, a.StaffId,
            a.Status.ToString(), a.CreatedAt,
            a.Branch?.Name, a.ServiceType?.Name, a.Staff?.FullName,
            a.Slot?.StartAt, a.Slot?.EndAt);

    public static List<AppointmentResponse> ToResponse(this List<Appointment> list)
        => list.Select(a => a.ToResponse()).ToList();

    public static AuditLogResponse ToResponse(this AuditLog a)
        => new(a.Id, a.ActorId, a.Role.Title, a.ActionTypeId,
            a.EntityTypeId, a.EntityId, a.Timestamp, a.MetadataJson);

    public static List<AuditLogResponse> ToResponse(this List<AuditLog> list)
        => list.Select(a => a.ToResponse()).ToList();
}
