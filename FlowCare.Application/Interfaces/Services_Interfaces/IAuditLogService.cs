using FlowCare.Domain.Entities;

namespace FlowCare.Application.Interfaces.Services;

public interface IAuditLogService
{
    Task<List<AuditLog>> GetAllPaginatedAsync(int skip, int take, CancellationToken cancellationToken = default);
    Task<List<AuditLog>> GetByBranchIdPaginatedAsync(int branchId, int skip, int take, CancellationToken cancellationToken = default);
    Task<AuditLog> LogAsync(AuditLog auditLog, CancellationToken cancellationToken = default);
}
