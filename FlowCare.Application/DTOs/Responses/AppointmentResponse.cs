using FlowCare.Domain.Enums;

namespace FlowCare.Application.DTOs.Responses;

public sealed record AppointmentResponse(
    int Id,
    int CustomerId,
    int BranchId,
    int ServiceTypeId,
    int SlotId,
    int StaffId,
    string Status,
    DateTimeOffset CreatedAt,
    string? BranchName,
    string? ServiceTypeName,
    string? StaffName,
    DateTimeOffset? SlotStartAt,
    DateTimeOffset? SlotEndAt);
