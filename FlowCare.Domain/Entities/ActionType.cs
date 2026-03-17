namespace FlowCare.Domain.Entities;

public sealed class ActionType
{
    public int Id { get; init; }
    public required string Type { get; init; }

    public ICollection<AuditLog> AuditLogs { get; init; } = new List<AuditLog>();
}
