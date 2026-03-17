using FlowCare.Domain.Entities;

namespace FlowCare.Application.Interfaces.Persistence;

public interface ICustomerDb
{
    Task<Customer> CreateCustomerAsync(Customer customer, CancellationToken cancellationToken = default);
    Task<Customer?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
    Task<Customer?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    
    Task<List<Customer>> GetAllCustomersPaginatedAsync(int skip, int take, string? term = null, CancellationToken cancellationToken = default);
    Task<int> CountAllAsync(string? term = null, CancellationToken cancellationToken = default);

    Task<List<Customer>> GetAllCustomerByBranchIdPaginatedAsync(int branchId, int skip, int take, string? term = null, CancellationToken cancellationToken = default);
    Task<int> CountByBranchIdAsync(int branchId, string? term = null, CancellationToken cancellationToken = default);

    Task<Customer?> GetByIdAsync(int customerId, CancellationToken cancellationToken = default);
    Task<Customer> UpdateDetailsAsync(Customer customer, CancellationToken cancellationToken = default);
}
