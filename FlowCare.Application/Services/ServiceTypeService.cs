using FlowCare.Application.Interfaces.Persistence;
using FlowCare.Application.Interfaces.Services;
using FlowCare.Domain.Entities;

namespace FlowCare.Application.Services;

public sealed class ServiceTypeService : IServiceTypeService
{
    private readonly IServiceTypeDb _serviceTypeDb;

    public ServiceTypeService(IServiceTypeDb serviceTypeDb)
    {
        _serviceTypeDb = serviceTypeDb;
    }

    public async Task<List<ServiceType>> GetServicesByBranchIdAsync(int branchId, CancellationToken cancellationToken = default)
    {
        return await _serviceTypeDb.GetServicesByBranchIdAsync(branchId, cancellationToken);
    }

    public async Task<ServiceType?> GetServiceByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _serviceTypeDb.GetServiceByIdAsync(id, cancellationToken);
    }

    public async Task<ServiceType> CreateServiceAsync(ServiceType serviceType, CancellationToken cancellationToken = default)
    {
        return await _serviceTypeDb.CreateServiceAsync(serviceType, cancellationToken);
    }

    public async Task<ServiceType> UpdateServiceAsync(ServiceType serviceType, CancellationToken cancellationToken = default)
    {
        return await _serviceTypeDb.UpdateServiceAsync(serviceType, cancellationToken);
    }
}
