using FlowCare.Domain.Entities;

namespace FlowCare.Application.Interfaces.Persistence;

public interface IAuditLogDb
{
    Task<AuditLog> CreateAsync(AuditLog auditLog, CancellationToken cancellationToken = default);
    Task<List<AuditLog>> GetAllPaginatedAsync(int skip, int take, CancellationToken cancellationToken = default);
    Task<List<AuditLog>> GetByBranchIdPaginatedAsync(int branchId, int skip, int take, CancellationToken cancellationToken = default);
}
