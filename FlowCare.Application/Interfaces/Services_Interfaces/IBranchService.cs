using FlowCare.Application.Pagination;
using FlowCare.Domain.Entities;

namespace FlowCare.Application.Interfaces.Services;

public interface IBranchService
{
    Task<Branch> CreateBranchAsync(Branch branch, CancellationToken cancellationToken = default);
    Task<Branch?> GetBranchByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<PagedResponse<List<Branch>>> GetAllBranchesPaginatedAsync(int skip, int take,string? term = null, CancellationToken cancellationToken = default);
    Task<Branch> UpdateBranchAsync(Branch branch, CancellationToken cancellationToken = default);
}
