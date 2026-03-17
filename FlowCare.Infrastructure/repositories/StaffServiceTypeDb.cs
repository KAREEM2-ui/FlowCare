using FlowCare.Application.Interfaces.Persistence;
using FlowCare.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlowCare.Infrastructure.Repositories;

public sealed class StaffServiceTypeDb : IStaffServiceTypeDb
{
    private readonly AppDbContext _db;

    public StaffServiceTypeDb(AppDbContext db)
    {
        _db = db;
    }

    public async Task<StaffServiceType> AssignAsync(int staffId, int serviceTypeId, CancellationToken cancellationToken = default)
    {
        var exists = await _db.StaffServiceTypes
            .AnyAsync(x => x.StaffId == staffId && x.ServiceTypeId == serviceTypeId, cancellationToken);

        if (exists)
            throw new InvalidOperationException($"Staff '{staffId}' is already assigned to service type '{serviceTypeId}'");

        var entry = new StaffServiceType
        {
            StaffId = staffId,
            ServiceTypeId = serviceTypeId
        };

        _db.StaffServiceTypes.Add(entry);
        await _db.SaveChangesAsync(cancellationToken);
        return entry;
    }

    public async Task<List<ServiceType>> GetServicesOfStaffByStaffIdAsync(int staffId, CancellationToken cancellationToken = default)
    {
        return await _db.StaffServiceTypes
            .AsNoTracking()
            .Where(x => x.StaffId == staffId)
            .Select(x => x.ServiceType!)
            .ToListAsync(cancellationToken);
    }
}
