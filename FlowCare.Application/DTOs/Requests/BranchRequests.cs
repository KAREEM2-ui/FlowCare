namespace FlowCare.Application.DTOs.Requests;

public sealed record CreateBranchRequest(
    string Name,
    string City,
    string Address,
    string Timezone);

public sealed record UpdateBranchRequest(
    string Name,
    string City,
    string Address,
    string Timezone);
