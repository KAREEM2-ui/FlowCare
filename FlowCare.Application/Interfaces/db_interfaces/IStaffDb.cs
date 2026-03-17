using FlowCare.Domain.Entities;

namespace FlowCare.Application.Interfaces.Persistence;

public interface IStaffDb
{
    Task<Staff> CreateStaffAsync(Staff staff, CancellationToken cancellationToken = default);
    Task<Staff?> GetByIdAsync(int staffId, CancellationToken cancellationToken = default);
    Task<Staff?> GetByUsernameAsync(string username, CancellationToken cancellationToken = default);
    Task<Staff?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);
    Task<Staff> UpdateStaffDetailsAsync(Staff staff, CancellationToken cancellationToken = default);
    Task<Staff> ChangeStaffBranchAsync(int staffId, int newBranchId, CancellationToken cancellationToken = default);
    
    Task<List<Staff>> GetAllStaffPaginatedAsync(int skip, int take, string? term = null, CancellationToken cancellationToken = default);
    Task<int> CountAllAsync(string? term = null, CancellationToken cancellationToken = default);

    Task<List<Staff>> GetStaffByBranchIdPaginatedAsync(int branchId, int skip, int take, string? term = null, CancellationToken cancellationToken = default);
    Task<int> CountByBranchIdAsync(int branchId, string? term = null, CancellationToken cancellationToken = default);
    Task<int> CountStaffByBranchIdAsync(int branchId, string? term,CancellationToken ct = default);
}
