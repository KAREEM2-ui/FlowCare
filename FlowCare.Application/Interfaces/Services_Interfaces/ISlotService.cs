using FlowCare.Application.DTOs.Requests;
using FlowCare.Application.Pagination;
using FlowCare.Domain.Entities;

namespace FlowCare.Application.Interfaces.Services;

public interface ISlotService
{
    Task<Slot> CreateSlotAsync(Slot slot, CancellationToken cancellationToken = default);
    Task CreateBulkAsync(List<Slot> slots, CancellationToken cancellationToken = default);
    Task<Slot> UpdateSlotAsync(Slot slot, UpdateSlotRequest updateSlotRequest, CancellationToken cancellationToken = default);
    Task SoftDeleteAsync(Slot slot, CancellationToken cancellationToken = default);
    Task<List<Slot>> HardDeleteExpiredAsync(CancellationToken cancellationToken = default);
    Task<PagedResponse<List<Slot>>> GetSlotsByBranchIdPaginatedAsync(int branchId, int skip, int take, CancellationToken cancellationToken = default);
    Task<Slot?> GetByIdAsync(int slotId, CancellationToken cancellationToken = default);
    Task<List<Slot>> GetAvailableSlotsByBranchAndServiceAsync(int branchId, int serviceTypeId, DateOnly? date, CancellationToken cancellationToken = default);
}
