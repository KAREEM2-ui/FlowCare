using FlowCare.Application.Interfaces.Persistence;
using FlowCare.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlowCare.Infrastructure.Repositories;

public sealed class StaffDb : IStaffDb
{
    private readonly AppDbContext _context;

    public StaffDb(AppDbContext context) { _context = context; }

    public async Task<int> CountAllAsync(CancellationToken cancellationToken = default)
       => await _context.Staff.CountAsync(cancellationToken);


        

    public async Task<Staff> CreateStaffAsync(Staff staff, CancellationToken cancellationToken = default)
    {
        _context.Staff.Add(staff);
        await _context.SaveChangesAsync(cancellationToken);
        return staff;
    }

    public async Task<Staff?> GetByIdAsync(int staffId, CancellationToken cancellationToken = default)
    {
        return await _context.Staff.FindAsync(new object[] { staffId }, cancellationToken);
    }

    public async Task<Staff?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return await _context.Staff.FirstOrDefaultAsync(s => s.Username == username, cancellationToken);
    }

    public async Task<Staff?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Staff.FirstOrDefaultAsync(s => s.Email == email, cancellationToken);
    }

    public async Task<Staff> UpdateStaffDetailsAsync(Staff staff, CancellationToken cancellationToken = default)
    {
        _context.Staff.Update(staff);
        await _context.SaveChangesAsync(cancellationToken);
        return staff;
    }

    public async Task<Staff> ChangeStaffBranchAsync(int staffId, int newBranchId, CancellationToken cancellationToken = default)
    {
        var existing = await _context.Staff.FirstOrDefaultAsync(s => s.Id == staffId, cancellationToken)
            ?? throw new KeyNotFoundException($"Staff '{staffId}' not found");

        existing.BranchId = newBranchId;
        await _context.SaveChangesAsync(cancellationToken);

        return (await _context.Staff.AsNoTracking()
            .Include(s => s.Branch)
            .FirstAsync(s => s.Id == staffId, cancellationToken));
    }

    public async Task<int> CountAllAsync(string? term = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Staff.AsQueryable();
        if (!string.IsNullOrWhiteSpace(term))
        {
            var likeTerm = $"%{term}%";
            query = query.Where(s => 
                EF.Functions.ILike(s.FullName, likeTerm) || 
                EF.Functions.ILike(s.Email, likeTerm) || 
                EF.Functions.ILike(s.Username, likeTerm));
        }
        return await query.CountAsync(cancellationToken);
    }

    public async Task<List<Staff>> GetAllStaffPaginatedAsync(int skip, int take, string? term = null, CancellationToken cancellationToken = default)
    {
        var query = _context.Staff.AsQueryable();
        
        // Apply Search by term if provided
        if (!string.IsNullOrWhiteSpace(term))
        {
            var likeTerm = $"%{term}%";
            query = query.Where(s => 
                EF.Functions.ILike(s.FullName, likeTerm) || 
                EF.Functions.ILike(s.Email, likeTerm) || 
                EF.Functions.ILike(s.Username, likeTerm));
        }

        // Apply Pagination
        return await query.OrderBy(s => s.Id).Skip(skip).Take(take).ToListAsync(cancellationToken);
    }



    private IQueryable<Staff> TryApplyTermFiltering(string? term)
    {
        IQueryable<Staff> query = _context.Staff.AsQueryable();


        if (term is null)
            return query;


        var likeTerm = $"%{term}%";
        query = query.Where(s =>
            EF.Functions.ILike(s.FullName, likeTerm) ||
            EF.Functions.ILike(s.Email, likeTerm) ||
            EF.Functions.ILike(s.Username, likeTerm));

        return query;
    }
    public async Task<List<Staff>> GetStaffByBranchIdPaginatedAsync(int branchId, int skip, int take, string? term = null, CancellationToken cancellationToken = default)
    {

        
        return await TryApplyTermFiltering(term)
            .Where(s => s.BranchId == branchId)
            .OrderBy(s => s.Id)
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> CountByBranchIdAsync(int branchId, string? term = null, CancellationToken cancellationToken = default)
    {

        return await TryApplyTermFiltering(term).CountAsync(s => s.BranchId == branchId, cancellationToken);
    }


    public async Task<int> CountStaffByBranchIdAsync(int branchId,string? term, CancellationToken ct = default)
    {
        return await TryApplyTermFiltering(term).Where(s => s.BranchId == branchId).CountAsync(ct);
    }
}
