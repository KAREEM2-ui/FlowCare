using FlowCare.Application.DTOs.Requests;
using FlowCare.Application.Interfaces.Persistence;
using FlowCare.Application.Interfaces.Services;
using FlowCare.Application.Pagination;
using FlowCare.Domain.Entities;

namespace FlowCare.Application.Services;

public sealed class SlotService : ISlotService
{
    private readonly ISlotDb _slotDb;

    public SlotService(ISlotDb slotDb)
    {
        _slotDb = slotDb;
    }

    public async Task<Slot> CreateSlotAsync(Slot slot, CancellationToken cancellationToken = default)
    {
        return await _slotDb.CreateSlotAsync(slot, cancellationToken);
    }

    public async Task CreateBulkAsync(List<Slot> slots, CancellationToken cancellationToken = default)
    {
        await _slotDb.CreateBulkAsync(slots, cancellationToken);
    }

    public async Task<Slot> UpdateSlotAsync(Slot slot, UpdateSlotRequest updateSlotRequest, CancellationToken cancellationToken = default)
    {
        slot.StartAt = updateSlotRequest.StartAt;
        slot.EndAt = updateSlotRequest.EndAt;
        slot.ServiceTypeId = updateSlotRequest.ServiceTypeId;
        slot.StaffId = updateSlotRequest.StaffId;
        slot.Capacity = updateSlotRequest.Capacity;

        return await _slotDb.UpdateSlotAsync(slot, cancellationToken);
    }

    public async Task SoftDeleteAsync(Slot slot, CancellationToken cancellationToken = default)
    {
        await _slotDb.SoftDeleteAsync(slot, cancellationToken);
    }

    public async Task<List<Slot>> HardDeleteExpiredAsync(CancellationToken cancellationToken = default)
    {
        var expiredSlots = await _slotDb.GetExpiredSoftDeletedSlotsAsync(cancellationToken);
        if (expiredSlots.Count == 0)
            return expiredSlots;

        await _slotDb.HardDeleteExpiredAsync(expiredSlots, cancellationToken);
        return expiredSlots;
    }

    public async Task<PagedResponse<List<Slot>>> GetSlotsByBranchIdPaginatedAsync(int branchId, int skip, int take, CancellationToken cancellationToken = default)
    {
        var records = await _slotDb.GetSlotsByBranchIdPaginatedAsync(branchId, skip, take, cancellationToken);
        var total = await _slotDb.CountByBranchIdAsync(branchId, cancellationToken);

        return new PagedResponse<List<Slot>> { Result = records, Total = total };
    }

    public async Task<Slot?> GetByIdAsync(int slotId, CancellationToken cancellationToken = default)
    {
        return await _slotDb.GetByIdAsync(slotId, cancellationToken);
    }

    public async Task<List<Slot>> GetAvailableSlotsByBranchAndServiceAsync(int branchId, int serviceTypeId, DateOnly? date, CancellationToken cancellationToken = default)
    {
        return await _slotDb.GetAvailableSlotsByBranchAndServiceAsync(branchId, serviceTypeId, date, cancellationToken);
    }
}
