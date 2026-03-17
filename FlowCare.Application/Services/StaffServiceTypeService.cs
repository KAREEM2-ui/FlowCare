using FlowCare.Application.Interfaces.Persistence;
using FlowCare.Application.Interfaces.Services;
using FlowCare.Domain.Entities;

namespace FlowCare.Application.Services;

public sealed class StaffServiceTypeService : IStaffServiceTypeService
{
    private readonly IStaffServiceTypeDb _db;

    public StaffServiceTypeService(IStaffServiceTypeDb db)
    {
        _db = db;
    }

    public async Task<StaffServiceType> AssignAsync(int staffId, int serviceTypeId, CancellationToken cancellationToken = default)
    {
        return await _db.AssignAsync(staffId, serviceTypeId, cancellationToken);
    }

    public async Task<List<ServiceType>> GetServicesOfStaffByStaffIdAsync(int staffId, CancellationToken cancellationToken = default)
    {
        return await _db.GetServicesOfStaffByStaffIdAsync(staffId, cancellationToken);
    }
}
