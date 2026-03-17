using FlowCare.Application.Interfaces.Persistence;
using FlowCare.Application.Interfaces.Services;
using FlowCare.Application.Pagination;
using FlowCare.Domain.Entities;

namespace FlowCare.Application.Services;

public sealed class CustomerService : ICustomerService
{
    private readonly ICustomerDb _customerDb;

    public CustomerService(ICustomerDb customerDb)
    {
        _customerDb = customerDb;
    }

    public async Task<PagedResponse<List<Customer>>> GetAllCustomerByBranchIdPaginatedAsync(int branchId, int skip, int take,string? term, CancellationToken cancellationToken = default)
    {
        var result = await _customerDb.GetAllCustomerByBranchIdPaginatedAsync(branchId, skip, take, term, cancellationToken);
        int total = await _customerDb.CountByBranchIdAsync(branchId, term, cancellationToken);


        return new PagedResponse<List<Customer>>()
        {
            Result = result,
            Total = total
        };
    }

    public async Task<PagedResponse<List<Customer>>> GetAllCustomersPaginatedAsync(int skip, int take, string? term,CancellationToken cancellationToken = default)
    {
         var result = await _customerDb.GetAllCustomersPaginatedAsync(skip, take, term,cancellationToken);
         int total = await _customerDb.CountAllAsync(term, cancellationToken);


        return new PagedResponse<List<Customer>>()
        {
            Result = result,
            Total = total
        };
    }


    public async Task<Customer?> GetByIdAsync(int customerId, CancellationToken cancellationToken = default)
    {
        return await _customerDb.GetByIdAsync(customerId, cancellationToken);
    }

    public async Task<Customer> UpdateDetailsAsync(Customer customer, CancellationToken cancellationToken = default)
    {
        return await _customerDb.UpdateDetailsAsync(customer, cancellationToken);
    }


}
