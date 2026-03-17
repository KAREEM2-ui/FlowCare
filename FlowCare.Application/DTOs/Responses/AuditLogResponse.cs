namespace FlowCare.Application.DTOs.Responses;

public sealed record AuditLogResponse(
    int Id,
    int ActorId,
    string ActorRole,
    int ActionTypeId,
    int EntityTypeId,
    int EntityId,
    DateTimeOffset Timestamp,
    string? MetadataJson);
