namespace FlowCare.Application.DTOs.Requests;

public sealed record CreateSlotRequest(
    int BranchId,
    int ServiceTypeId,
    int StaffId,
    DateTimeOffset StartAt,
    DateTimeOffset EndAt,
    int Capacity = 1);

public sealed record UpdateSlotRequest(
    int ServiceTypeId,
    int StaffId,
    DateTimeOffset StartAt,
    DateTimeOffset EndAt,
    int Capacity = 1);

public sealed record BulkCreateSlotRequest(List<CreateSlotRequest> Slots);
