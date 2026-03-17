using FlowCare.Application.Interfaces.Persistence;
using FlowCare.Application.Interfaces.Services;
using FlowCare.Application.Pagination;
using FlowCare.Domain.Entities;

namespace FlowCare.Application.Services;

public sealed class StaffService : IStaffService
{
    private readonly IStaffDb _staffDb;

    public StaffService(IStaffDb staffDb)
    {
        _staffDb = staffDb;
    }

    public async Task<Staff> CreateStaffAsync(Staff staff, CancellationToken cancellationToken = default)
    {
        return await _staffDb.CreateStaffAsync(staff, cancellationToken);
    }

    public async Task<Staff?> GetByIdAsync(int staffId, CancellationToken cancellationToken = default)
    {
        return await _staffDb.GetByIdAsync(staffId, cancellationToken);
    }

    public async Task<Staff> UpdateStaffDetailsAsync(Staff staff, CancellationToken cancellationToken = default)
    {
        return await _staffDb.UpdateStaffDetailsAsync(staff, cancellationToken);
    }

    public async Task<Staff> ChangeStaffBranchAsync(int staffId, int newBranchId, CancellationToken cancellationToken = default)
    {
        return await _staffDb.ChangeStaffBranchAsync(staffId, newBranchId, cancellationToken);
    }

    public async Task<PagedResponse<List<Staff>>> GetAllStaffPaginatedAsync(int skip, int take, string? term = null, CancellationToken cancellationToken = default)
    {
        var records = await _staffDb.GetAllStaffPaginatedAsync(skip, take, term, cancellationToken);
        var total = await _staffDb.CountAllAsync(term, cancellationToken);
        
        return new PagedResponse<List<Staff>> { Result = records, Total = total };
    }

    public async Task<PagedResponse<List<Staff>>>  GetStaffByBranchIdPaginatedAsync(int branchId, int skip, int take, string? term,CancellationToken cancellationToken = default)
    {
        var records =  await _staffDb.GetStaffByBranchIdPaginatedAsync(branchId, skip, take, term,cancellationToken);
        int total = await _staffDb.CountByBranchIdAsync(branchId,term,cancellationToken);

        return new PagedResponse<List<Staff>>{ Result = records, Total = total };
    }

   
}
