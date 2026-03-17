using FlowCare.Application.DTOs.Responses;
using FlowCare.Application.Interfaces.Persistence;
using FlowCare.Domain.Entities;
using FlowCare.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace FlowCare.Infrastructure.Repositories;

public sealed class AppointmentDb : IAppointmentDb
{
    private readonly AppDbContext _context;

    public AppointmentDb(AppDbContext context)
    {
        _context = context;
    }

    public async Task<int> CountByBranchIdAsync(int branchId, CancellationToken cancellationToken = default)
        => await _context.Appointments.CountAsync(a => a.BranchId == branchId, cancellationToken);

    public async Task<int> CountByCustomerIdAsync(int customerId, CancellationToken cancellationToken = default)
        => await _context.Appointments.CountAsync(a => a.CustomerId == customerId, cancellationToken);

    public async Task<int> CountByStaffIdAsync(int staffId, CancellationToken cancellationToken = default)
       => await _context.Appointments.CountAsync(a => a.StaffId == staffId, cancellationToken);

    public async Task<Appointment?> GetByIdAsync(int appointmentId, CancellationToken cancellationToken = default)
    {
        return await _context.Appointments.FindAsync(new object[] { appointmentId }, cancellationToken);
    }

    public async Task<Appointment> CreateAsync(Appointment appointment, CancellationToken cancellationToken = default)
    {
        _context.Appointments.Add(appointment);
        await _context.SaveChangesAsync(cancellationToken);
        return appointment;
    }

    public async Task<Appointment> UpdateStatusAsync(int appointmentId, AppointmentStatus newStatus, CancellationToken cancellationToken = default)
    {
        var apt = await GetByIdAsync(appointmentId, cancellationToken);
        if (apt != null) { apt.Status = newStatus; await _context.SaveChangesAsync(cancellationToken); }
        return apt!;
    }

    public async Task<Appointment> UpdateSlotAsync(int appointmentId, int newSlotId, CancellationToken cancellationToken = default)
    {
        var apt = await GetByIdAsync(appointmentId, cancellationToken);
        if (apt != null) { apt.SlotId = newSlotId; await _context.SaveChangesAsync(cancellationToken); }
        return apt!;
    }

   
    private IQueryable<Appointment> TryApplyTermFiltering(string? term)
    {
        IQueryable<Appointment> query = _context.Appointments.AsQueryable();


        if (string.IsNullOrWhiteSpace(term))
            return query;

        var likeTerm = $"%{term}%";
        return query.Where(a =>
            (EF.Functions.ILike(a.Customer!.FullName, likeTerm)) ||
            (EF.Functions.ILike(a.Customer.Email, likeTerm)) ||
            (EF.Functions.ILike(a.ServiceType!.Name, likeTerm)));
    }

    public async Task<int> CountAllByBranchIdAsync(int branchId, string? term = null, CancellationToken cancellationToken = default)
        => await TryApplyTermFiltering(term).CountAsync(cancellationToken);

    public async Task<int> CountAllByCustomerIdAsync(int customerId, string? term = null, CancellationToken cancellationToken = default)
        => await TryApplyTermFiltering(term).CountAsync(cancellationToken);

    public async Task<int> CountAllByStaffIdAsync(int staffId, string? term = null, CancellationToken cancellationToken = default)
       => await TryApplyTermFiltering(term).CountAsync(cancellationToken);

    public async Task<int> CountAllAsync(string? term = null, CancellationToken ct = default)
       => await TryApplyTermFiltering(term).CountAsync(ct);

    public async Task<List<Appointment>> GetAppointmentsByDateDescByBranchIdPaginatedAsync(int branchId, int skip, int take, string? term = null, CancellationToken cancellationToken = default)
        => await TryApplyTermFiltering(term).OrderByDescending(a => a.CreatedAt).Skip(skip).Take(take).ToListAsync(cancellationToken);

    public async Task<List<Appointment>> GetByCustomerIdPaginatedAsync(int customerId, int skip, int take, string? term = null, CancellationToken cancellationToken = default)
        => await TryApplyTermFiltering(term).OrderByDescending(a => a.CreatedAt).Skip(skip).Take(take).ToListAsync(cancellationToken);

    public async Task<List<Appointment>> GetByStaffIdPaginatedAsync(int staffId, int skip, int take, string? term = null, CancellationToken cancellationToken = default)
        => await TryApplyTermFiltering(term).OrderByDescending(a => a.CreatedAt).Skip(skip).Take(take).ToListAsync(cancellationToken);

    public async Task<List<Appointment>> GetAllPaginatedAsync(int skip, int take, string? term = null, CancellationToken cancellationToken = default)
       => await TryApplyTermFiltering(term).OrderByDescending(a => a.CreatedAt).Skip(skip).Take(take).ToListAsync(cancellationToken);

    public async Task<List<BranchSlotPosition>> GetBranchSlotsPositionsAsync(int branchId, CancellationToken cancellationToken = default)
    {
        return await _context.branchSlotPositions
            .FromSqlInterpolated($"""SELECT * FROM "GetBranchSlotsPositions"({branchId})""")
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
}
