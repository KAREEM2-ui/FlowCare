using FlowCare.Domain.Entities;

namespace FlowCare.Application.Interfaces.Persistence;

public interface IStaffServiceTypeDb
{
    Task<StaffServiceType> AssignAsync(int staffId, int serviceTypeId, CancellationToken cancellationToken = default);
    Task<List<ServiceType>> GetServicesOfStaffByStaffIdAsync(int staffId, CancellationToken cancellationToken = default);
}
