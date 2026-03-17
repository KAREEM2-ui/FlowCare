using FlowCare.Domain.Entities;

namespace FlowCare.Application.Interfaces.Persistence;

public interface ISlotDb
{
    Task<Slot> CreateSlotAsync(Slot slot, CancellationToken cancellationToken = default);
    Task CreateBulkAsync(List<Slot> slots, CancellationToken cancellationToken = default);
    Task<Slot?> GetByIdAsync(int slotId, CancellationToken cancellationToken = default);
    Task<Slot> UpdateSlotAsync(Slot slot, CancellationToken cancellationToken = default);
    Task SoftDeleteAsync(Slot slot, CancellationToken cancellationToken = default);

    Task<List<Slot>> GetExpiredSoftDeletedSlotsAsync(CancellationToken cancellationToken = default);
    Task<int> HardDeleteExpiredAsync(List<Slot> slotsToDelete, CancellationToken cancellationToken = default);

    Task<List<Slot>> GetSlotsByBranchIdPaginatedAsync(int branchId, int skip, int take, CancellationToken cancellationToken = default);
    Task<int> CountByBranchIdAsync(int branchId, CancellationToken cancellationToken = default);
    Task<List<Slot>> GetAvailableSlotsByBranchAndServiceAsync(int branchId, int serviceTypeId, DateOnly? date, CancellationToken cancellationToken = default);
}
