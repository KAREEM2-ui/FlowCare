using FlowCare.Domain.Entities;

namespace FlowCare.Application.Interfaces.Persistence;

public interface IBranchDb
{
    Task<Branch> CreateBranchAsync(Branch branch, CancellationToken cancellationToken = default);
    Task<Branch?> GetBranchByIdAsync(int id, CancellationToken cancellationToken = default);
    
    Task<List<Branch>> GetAllBranchesPaginatedAsync(int skip, int take, string? term = null, CancellationToken cancellationToken = default);
    Task<int> CountAllAsync(string? term = null, CancellationToken cancellationToken = default);

    Task<Branch> UpdateBranchAsync(Branch branch, CancellationToken cancellationToken = default);
}
