using FlowCare.Application.Interfaces.Persistence;
using FlowCare.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlowCare.Infrastructure.Repositories;

public sealed class ServiceTypeDb : IServiceTypeDb
{
    private readonly AppDbContext _db;

    public ServiceTypeDb(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<ServiceType>> GetServicesByBranchIdAsync(int branchId, CancellationToken cancellationToken = default)
    {
        return await _db.ServiceTypes
            .AsNoTracking()
            .Where(s => s.BranchId == branchId)
            .OrderBy(s => s.Name)
            .ToListAsync(cancellationToken);
    }

    public async Task<ServiceType?> GetServiceByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _db.ServiceTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(s => s.Id == id, cancellationToken);
    }

    public async Task<ServiceType> CreateServiceAsync(ServiceType serviceType, CancellationToken cancellationToken = default)
    {
        _db.ServiceTypes.Add(serviceType);
        await _db.SaveChangesAsync(cancellationToken);
        return serviceType;
    }

    public async Task<ServiceType> UpdateServiceAsync(ServiceType serviceType, CancellationToken cancellationToken = default)
    {
        _db.ServiceTypes.Update(serviceType);
        await _db.SaveChangesAsync(cancellationToken);
        return serviceType;
    }
}
