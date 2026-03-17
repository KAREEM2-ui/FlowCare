using FlowCare.Application.Pagination;
using FlowCare.Domain.Entities;

namespace FlowCare.Application.Interfaces.Services;

public interface IStaffService
{
    Task<Staff> CreateStaffAsync(Staff staff, CancellationToken cancellationToken = default);
    Task<PagedResponse<List<Staff>>> GetAllStaffPaginatedAsync(int skip, int take,string? term, CancellationToken cancellationToken = default);
    Task<PagedResponse<List<Staff>>> GetStaffByBranchIdPaginatedAsync(int branchId, int skip, int take,string? term, CancellationToken cancellationToken = default);
    Task<Staff?> GetByIdAsync(int staffId, CancellationToken cancellationToken = default);
    Task<Staff> UpdateStaffDetailsAsync(Staff staff, CancellationToken cancellationToken = default);
    Task<Staff> ChangeStaffBranchAsync(int staffId, int newBranchId, CancellationToken cancellationToken = default);
}
