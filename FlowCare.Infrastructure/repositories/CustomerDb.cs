using FlowCare.Application.Interfaces.Persistence;
using FlowCare.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlowCare.Infrastructure.Repositories;

public sealed class CustomerDb : ICustomerDb
{
    private readonly AppDbContext _context;

    public CustomerDb(AppDbContext context) { _context = context; }
    private IQueryable<Customer> TryApplyTermFiltering( string? term)
    {
        IQueryable<Customer> query = _context.Customers.AsQueryable();
        if (string.IsNullOrWhiteSpace(term))
            return query;

        var likeTerm = $"%{term}%";
        return query.Where(c =>
            EF.Functions.ILike(c.FullName, likeTerm) ||
            EF.Functions.ILike(c.Email, likeTerm) ||
            (c.Phone != null && EF.Functions.ILike(c.Phone, likeTerm)) ||
            EF.Functions.ILike(c.Username, likeTerm));
    }
    public async Task<int> CountAllAsync(string? term,CancellationToken cancellationToken = default)
        => await TryApplyTermFiltering(term).CountAsync(cancellationToken);

    public async Task<int> CountByBranchIdAsync(int branchId, string? term,CancellationToken cancellationToken = default)
        => await TryApplyTermFiltering(term).Where(c => c.Appointments.Any(a => a.BranchId == branchId)).Select(c => c.Id).Distinct().CountAsync(cancellationToken);

    public async Task<Customer> CreateCustomerAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        _context.Customers.Add(customer);
        await _context.SaveChangesAsync(cancellationToken);
        return customer;
    }

    public async Task<Customer?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default)
    {
        return await _context.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Username == username, cancellationToken);
    }

    public async Task<Customer?> GetByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return await _context.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Email == email, cancellationToken);
    }

    public async Task<List<Customer>> GetAllCustomersPaginatedAsync(int skip, int take, string? term,CancellationToken cancellationToken = default)
    {
        return await TryApplyTermFiltering(term)
            .OrderBy(c => c.Id)
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Customer>> GetAllCustomerByBranchIdPaginatedAsync(int branchId, int skip, int take, string? term,CancellationToken cancellationToken = default)
    {
        return await TryApplyTermFiltering(term)
            .Where(c => c.Appointments.Any(a => a.BranchId == branchId))
            .Distinct()
            .OrderBy(c => c.Id)
            .Skip(skip)
            .Take(take)
            .ToListAsync(cancellationToken);
    }

    public async Task<Customer?> GetByIdAsync(int customerId, CancellationToken cancellationToken = default)
    {
        return await _context.Customers
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == customerId, cancellationToken);
    }

    public async Task<Customer> UpdateDetailsAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        _context.Customers.Update(customer);
        await _context.SaveChangesAsync(cancellationToken);
        return customer;
    }
}
