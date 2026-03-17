using FlowCare.Domain.Entities;

namespace FlowCare.Application.Interfaces.Persistence;

public interface IServiceTypeDb
{
    Task<List<ServiceType>> GetServicesByBranchIdAsync(int branchId, CancellationToken cancellationToken = default);
    Task<ServiceType?> GetServiceByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ServiceType> CreateServiceAsync(ServiceType serviceType, CancellationToken cancellationToken = default);
    Task<ServiceType> UpdateServiceAsync(ServiceType serviceType, CancellationToken cancellationToken = default);
}
