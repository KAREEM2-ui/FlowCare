using FlowCare.Application.Interfaces.Persistence;
using FlowCare.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace FlowCare.Infrastructure.Repositories;

public sealed class SlotDb : ISlotDb
{
    private readonly AppDbContext _db;

    public SlotDb(AppDbContext db)
    {
        _db = db;
    }

    public async Task<Slot> CreateSlotAsync(Slot slot, CancellationToken cancellationToken = default)
    {
        _db.Slots.Add(slot);
        await _db.SaveChangesAsync(cancellationToken);
        return slot;
    }

    public async Task CreateBulkAsync(List<Slot> slots, CancellationToken cancellationToken = default)
    {
        _db.Slots.AddRange(slots);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task<Slot?> GetByIdAsync(int slotId, CancellationToken cancellationToken = default)
    {
        return await _db.Slots
            .AsNoTracking()
            .Include(s => s.Branch)
            .Include(s => s.ServiceType)
            .Include(s => s.Staff)
            .FirstOrDefaultAsync(s => s.Id == slotId, cancellationToken);
    }

    public async Task<Slot> UpdateSlotAsync(Slot slot, CancellationToken cancellationToken = default)
    {
        _db.Slots.Update(slot);
        await _db.SaveChangesAsync(cancellationToken);
        return slot;
    }

    public async Task SoftDeleteAsync(Slot slot, CancellationToken cancellationToken = default)
    {
        slot.DeletedAt = DateTimeOffset.UtcNow;
        slot.IsActive = false;
        _db.Update(slot);
        await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<Slot>> GetExpiredSoftDeletedSlotsAsync(CancellationToken cancellationToken = default)
    {
        var now = DateTimeOffset.UtcNow;

        var softDeleted = await _db.Slots
            .Include(s => s.Branch)
            .Where(s => s.DeletedAt != null && !s.IsActive)
            .ToListAsync(cancellationToken);

        return softDeleted
            .Where(s => s.Branch != null && s.DeletedAt!.Value.AddDays(s.Branch.RetentionPerioud) <= now)
            .ToList();
    }

    public async Task<int> HardDeleteExpiredAsync(List<Slot> slotsToDelete, CancellationToken cancellationToken = default)
    {
        if (slotsToDelete.Count == 0)
            return 0;

        _db.Slots.RemoveRange(slotsToDelete);
        return await _db.SaveChangesAsync(cancellationToken);
    }

    public async Task<List<Slot>> GetSlotsByBranchIdPaginatedAsync(int branchId, int skip, int take, CancellationToken cancellationToken = default)
    {
        return await _db.Slots
            .AsNoTracking()
            .Where(s => s.BranchId == branchId)
            .OrderBy(s => s.StartAt)
            .Skip(skip)
            .Take(take)
            .Include(s => s.ServiceType)
            .Include(s => s.Staff)
            .ToListAsync(cancellationToken);
    }

    public async Task<List<Slot>> GetAvailableSlotsByBranchAndServiceAsync(int branchId, int serviceTypeId, DateOnly? date, CancellationToken cancellationToken = default)
    {
        var query = _db.Slots
            .AsNoTracking()
            .Where(s =>
                s.BranchId == branchId
                && s.ServiceTypeId == serviceTypeId
                && s.IsActive
                && s.Appointments.Count < s.Capacity);

        if (date.HasValue)
        {
            var dateStart = new DateTimeOffset(date.Value.ToDateTime(TimeOnly.MinValue), TimeSpan.Zero);
            var dateEnd = dateStart.AddDays(1);
            query = query.Where(s => s.StartAt >= dateStart && s.StartAt < dateEnd);
        }

        return await query
            .OrderBy(s => s.StartAt)
            .Include(s => s.Staff)
            .ToListAsync(cancellationToken);
    }

    public async Task<int> CountByBranchIdAsync(int branchId, CancellationToken cancellationToken = default)
    {
        return await _db.Slots.Where(s=> s.BranchId == branchId).CountAsync(cancellationToken);
    }
}
