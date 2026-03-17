namespace FlowCare.Application.DTOs.Responses;

public sealed record SlotResponse(
    int Id,
    int BranchId,
    int ServiceTypeId,
    int StaffId,
    DateTimeOffset StartAt,
    DateTimeOffset EndAt,
    int Capacity,
    bool IsActive,
    string? StaffName);
