using FlowCare.Application.Pagination;
using FlowCare.Domain.Entities;

namespace FlowCare.Application.Interfaces.Services;

public interface ICustomerService
{
    Task<PagedResponse<List<Customer>>> GetAllCustomersPaginatedAsync(int skip, int take, string? term,CancellationToken cancellationToken = default);
    Task<PagedResponse<List<Customer>>> GetAllCustomerByBranchIdPaginatedAsync(int branchId, int skip, int take, string? term, CancellationToken cancellationToken = default);
    Task<Customer?> GetByIdAsync(int customerId, CancellationToken cancellationToken = default);
    Task<Customer> UpdateDetailsAsync(Customer customer, CancellationToken cancellationToken = default);
}
