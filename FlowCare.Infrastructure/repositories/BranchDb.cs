using FlowCare.Application.Interfaces.Persistence;
using FlowCare.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlowCare.Infrastructure.Repositories;

public sealed class BranchDb : IBranchDb
{
    private readonly AppDbContext _context;

    public BranchDb(AppDbContext context) { _context = context; }

    public async Task<int> CountAllAsync(CancellationToken cancellationToken = default)
        => await _context.Branches.CountAsync(cancellationToken);

    public async Task<Branch> CreateBranchAsync(Branch branch, CancellationToken cancellationToken = default)
    {
        _context.Branches.Add(branch);
        await _context.SaveChangesAsync(cancellationToken);
        return branch;
    }

    public async Task<Branch?> GetBranchByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _context.Branches
            .AsNoTracking()
            .FirstOrDefaultAsync(b => b.Id == id, cancellationToken);
    }

    private IQueryable<Branch> TryApplyTermFiltering(string? term)
    {
        IQueryable<Branch> query = _context.Branches.AsQueryable();


        if (string.IsNullOrWhiteSpace(term))
            return query;

        var likeTerm = $"%{term}%";
        return query.Where(b =>
            (EF.Functions.ILike(b.Name, likeTerm)) );
    }

    public async Task<List<Branch>> GetAllBranchesPaginatedAsync(int skip, int take,string? term = null, CancellationToken cancellationToken = default)
    {
        return await TryApplyTermFiltering(term)
            .AsNoTracking()
            .OrderBy(b => b.Name)
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public async Task<Branch> UpdateBranchAsync(Branch branch, CancellationToken cancellationToken = default)
    {
        _context.Branches.Update(branch);
        await _context.SaveChangesAsync(cancellationToken);
        return branch;
    }


    public async Task<int> CountAllAsync(string? term = null, CancellationToken cancellationToken = default)
    {
        return await TryApplyTermFiltering(term).CountAsync();
    }
}
