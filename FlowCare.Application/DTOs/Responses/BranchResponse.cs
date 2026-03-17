namespace FlowCare.Application.DTOs.Responses;

public sealed record BranchResponse(
    int Id,
    string Name,
    string City,
    string Address,
    string Timezone,
    bool IsActive);
