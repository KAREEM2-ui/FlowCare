namespace FlowCare.Application.DTOs.Responses;

public sealed record StaffResponse(
    int Id,
    string Username,
    string FullName,
    string Email,
    int BranchId,
    string? BranchName,
    string? RoleTitle,
    bool IsActive);
