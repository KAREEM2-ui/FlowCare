using FlowCare.Application.Interfaces.Persistence;
using FlowCare.Application.Interfaces.Services;
using FlowCare.Application.Pagination;
using FlowCare.Domain.Entities;

namespace FlowCare.Application.Services;

public sealed class BranchService : IBranchService
{
    private readonly IBranchDb _branchDb;

    public BranchService(IBranchDb branchDb)
    {
        _branchDb = branchDb;
    }

    public async Task<Branch> CreateBranchAsync(Branch branch, CancellationToken cancellationToken = default)
    {
        return await _branchDb.CreateBranchAsync(branch, cancellationToken);
    }

    public async Task<Branch?> GetBranchByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _branchDb.GetBranchByIdAsync(id, cancellationToken);
    }

    public async Task<PagedResponse<List<Branch>>> GetAllBranchesPaginatedAsync(int skip, int take, string? term,CancellationToken cancellationToken = default)
    {
        var records =  await _branchDb.GetAllBranchesPaginatedAsync(skip, take, term,cancellationToken);
        int total = await _branchDb.CountAllAsync(term,cancellationToken);

        return new PagedResponse<List<Branch>>() { Result = records, Total = total };
    }

    public async Task<Branch> UpdateBranchAsync(Branch branch, CancellationToken cancellationToken = default)
    {
        return await _branchDb.UpdateBranchAsync(branch, cancellationToken);
    }


}
