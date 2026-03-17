using FlowCare.Domain.Entities;

namespace FlowCare.Application.Interfaces.Services;

public interface IStaffServiceTypeService
{
    Task<StaffServiceType> AssignAsync(int staffId, int serviceTypeId, CancellationToken cancellationToken = default);
    Task<List<ServiceType>> GetServicesOfStaffByStaffIdAsync(int staffId, CancellationToken cancellationToken = default);
}
