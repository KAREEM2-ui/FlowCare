using FlowCare.Application.Interfaces.Persistence;
using FlowCare.Application.Interfaces.Services;
using FlowCare.Domain.Entities;

namespace FlowCare.Application.Services;

public sealed class AuditLogService : IAuditLogService
{
    private readonly IAuditLogDb _auditLogDb;

    public AuditLogService(IAuditLogDb auditLogDb)
    {
        _auditLogDb = auditLogDb;
    }

    public async Task<List<AuditLog>> GetAllPaginatedAsync(int skip, int take, CancellationToken cancellationToken = default)
    {
        return await _auditLogDb.GetAllPaginatedAsync(skip, take, cancellationToken);
    }

    public async Task<List<AuditLog>> GetByBranchIdPaginatedAsync(int branchId, int skip, int take, CancellationToken cancellationToken = default)
    {
        return await _auditLogDb.GetByBranchIdPaginatedAsync(branchId, skip, take, cancellationToken);
    }

    public async Task<AuditLog> LogAsync(AuditLog auditLog, CancellationToken cancellationToken = default)
    {
        return await _auditLogDb.CreateAsync(auditLog, cancellationToken);
    }
}
