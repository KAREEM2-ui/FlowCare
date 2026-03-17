namespace FlowCare.Application.DTOs.Responses;

public sealed record CustomerResponse(
    int Id,
    string Username,
    string FullName,
    string Email,
    string? Phone,
    bool IsActive);
