using FlowCare.Domain.Enums;

namespace FlowCare.Domain.Entities;

public sealed class AuditLog
{
    public  int Id { get; init; }

    public required int ActorId { get; init; }
    public int RoleId { get; init; }


    public int ActionTypeId { get; init; }
    public int EntityTypeId { get; init; }
    public required int EntityId { get; init; }

    public DateTimeOffset Timestamp { get; init; }
    public string? MetadataJson { get; init; }

    public ActionType ActionType { get; init; } = null!;
    public EntityType EntityType { get; init; } = null!;
    public Role Role { get; init; } = null!;
}
