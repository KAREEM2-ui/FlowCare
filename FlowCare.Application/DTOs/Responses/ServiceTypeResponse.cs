namespace FlowCare.Application.DTOs.Responses;

public sealed record ServiceTypeResponse(
    int Id,
    int BranchId,
    string Name,
    string? Description,
    int DurationMinutes,
    bool IsActive);
