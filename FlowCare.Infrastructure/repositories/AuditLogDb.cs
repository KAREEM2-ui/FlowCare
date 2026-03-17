using FlowCare.Application.Interfaces.Persistence;
using FlowCare.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlowCare.Infrastructure.Repositories;

public sealed class AuditLogDb : IAuditLogDb
{
    private readonly AppDbContext _db;

    public AuditLogDb(AppDbContext db)
    {
        _db = db;
    }

    public async Task<AuditLog> CreateAsync(AuditLog auditLog, CancellationToken cancellationToken = default)
    {
        _db.AuditLogs.Add(auditLog);
        await _db.SaveChangesAsync(cancellationToken);
        return auditLog;
    }

    public async Task<List<AuditLog>> GetAllPaginatedAsync(int skip, int take, CancellationToken cancellationToken = default)
    {
        return await _db.AuditLogs
            .AsNoTracking()
            .OrderByDescending(a => a.Timestamp)
            .Skip(skip)
            .Take(take)
            .Include(a => a.ActionType)
            .Include(a => a.EntityType)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<AuditLog>> GetByBranchIdPaginatedAsync(int branchId, int skip, int take, CancellationToken cancellationToken = default)
    {
        return await _db.AuditLogs
            .AsNoTracking()
            .Where(a => a.EntityId == branchId)
            .OrderByDescending(a => a.Timestamp)
            .Skip(skip)
            .Take(take)
            .Include(a => a.ActionType)
            .Include(a => a.EntityType)
            .ToListAsync(cancellationToken);
    }
}
