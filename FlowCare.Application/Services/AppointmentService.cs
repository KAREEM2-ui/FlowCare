using FlowCare.Application.DTOs.Requests;
using FlowCare.Application.Interfaces.Persistence;
using FlowCare.Application.Interfaces.Services;
using FlowCare.Application.Interfaces.Services_Interfaces;
using FlowCare.Application.Pagination;
using FlowCare.Domain.Entities;
using FlowCare.Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace FlowCare.Application.Services;

public sealed class AppointmentService : IAppointmentService
{
    private readonly IAppointmentDb _appointmentDb;
    private readonly IFileStorageService _fileStorageService;
    private readonly ICustomerRateLimiter _customerRateLimiter;

    public AppointmentService(
        IAppointmentDb appointmentDb,
        IFileStorageService fileStorageService,
        ICustomerRateLimiter customerRateLimiter)
    {
        _appointmentDb = appointmentDb;
        _fileStorageService = fileStorageService;
        _customerRateLimiter = customerRateLimiter;
    }

    public async Task<PagedResponse<List<Appointment>>> GetAppointmentsByDateDescByBranchIdPaginatedAsync(int branchId, int skip, int take, string? term, CancellationToken cancellationToken = default)
    {
        List<Appointment> appointments = await _appointmentDb.GetAppointmentsByDateDescByBranchIdPaginatedAsync(branchId, skip, take, term, cancellationToken);
        int totalCount = await _appointmentDb.CountAllByBranchIdAsync(branchId, term, cancellationToken);

        return new PagedResponse<List<Appointment>>()
        {
            Result = appointments,
            Total = totalCount
        };
    }

    public async Task<Appointment?> GetByIdAsync(int appointmentId, CancellationToken cancellationToken = default)
    {
        return await _appointmentDb.GetByIdAsync(appointmentId, cancellationToken);
    }

    public async Task<Appointment> BookAppointmentAsync(BookAppointmentRequest request, int customerId, CancellationToken cancellationToken = default)
    {
        if (!_customerRateLimiter.TryAcquireBooking(customerId, DateTimeOffset.UtcNow))
            throw new InvalidOperationException("Daily booking limit exceeded.");

        var appointment = new Appointment
        {
            CustomerId = customerId,
            BranchId = request.BranchId,
            ServiceTypeId = request.ServiceTypeId,
            SlotId = request.SlotId,
            StaffId = request.StaffId,
            Status = AppointmentStatus.Booked,
            CreatedAt = DateTimeOffset.UtcNow
        };

        if (request.formFile != null)
        {
            appointment.AttachementReference = await _fileStorageService.StoreAppointmentAttachmentAsync(request.formFile, cancellationToken);
        }

        try
        {
            return await _appointmentDb.CreateAsync(appointment, cancellationToken);
        }
        catch (DbUpdateException ex) when (IsSlotUniqueViolation(ex))
        {
            throw new InvalidOperationException("Slot already taken");
        }
    }

    public async Task<Appointment> CancelAppointmentAsync(int appointmentId, int customerId, CancellationToken cancellationToken = default)
    {
        var existing = await _appointmentDb.GetByIdAsync(appointmentId, cancellationToken);
        if (existing is null) throw new KeyNotFoundException("Appointment not found");
        if (existing.CustomerId != customerId) throw new UnauthorizedAccessException("You can only cancel your own appointment");
        if (existing.Status == AppointmentStatus.Cancelled) throw new InvalidOperationException("Appointment is already cancelled");

        return await _appointmentDb.UpdateStatusAsync(appointmentId, AppointmentStatus.Cancelled, cancellationToken);
    }

    public async Task<Appointment> RescheduleAppointmentAsync(int appointmentId, int customerId, int newSlotId, CancellationToken cancellationToken = default)
    {
        
        if (!_customerRateLimiter.TryAcquireReschedule(customerId, DateTimeOffset.UtcNow))
            throw new InvalidOperationException("Daily reschedule limit exceeded.");

        var existing = await _appointmentDb.GetByIdAsync(appointmentId, cancellationToken);
        if (existing is null) throw new KeyNotFoundException("Appointment not found");
        if (existing.CustomerId != customerId) throw new UnauthorizedAccessException("You can only reschedule your own appointment");
        if (existing.Status != AppointmentStatus.Booked) throw new InvalidOperationException("Only booked appointments can be rescheduled");

        try
        {
            return await _appointmentDb.UpdateSlotAsync(appointmentId, newSlotId, cancellationToken);
        }
        catch (DbUpdateException ex) when (IsSlotUniqueViolation(ex))
        {
            throw new InvalidOperationException("Slot already taken");
        }
    }

    public async Task<PagedResponse<List<Appointment>>> GetAppointmentHistoryByCustomerIdPaginatedAsync(int customerId, int skip, int take, string? term, CancellationToken cancellationToken = default)
    {
        var records = await _appointmentDb.GetByCustomerIdPaginatedAsync(customerId, skip, take, term, cancellationToken);
        var total = await _appointmentDb.CountAllByCustomerIdAsync(customerId, term, cancellationToken);

        return new PagedResponse<List<Appointment>>
        {
            Result = records,
            Total = total
        };
    }

    public async Task<PagedResponse<List<Appointment>>> GetAppointmentsByStaffIdPaginatedAsync(int staffId, int skip, int take, string? term, CancellationToken cancellationToken = default)
    {
        var all = await _appointmentDb.GetByStaffIdPaginatedAsync(staffId, skip, take, term, cancellationToken);
        int total = await _appointmentDb.CountAllByStaffIdAsync(staffId, term, cancellationToken);

        return new PagedResponse<List<Appointment>>()
        {
            Result = all,
            Total = total
        };
    }

    public async Task<Appointment> UpdateAppointmentStatusAsync(int appointmentId, AppointmentStatus newStatus, CancellationToken cancellationToken = default)
    {
        var existing = await _appointmentDb.GetByIdAsync(appointmentId, cancellationToken);
        if (existing is null) throw new KeyNotFoundException("Appointment not found");

        return await _appointmentDb.UpdateStatusAsync(appointmentId, newStatus, cancellationToken);
    }

    public async Task<PagedResponse<List<Appointment>>> GetAllAsyncPaginated(int skip, int take, string? term, CancellationToken cancellationToken = default)
    {
        List<Appointment> all = await _appointmentDb.GetAllPaginatedAsync(skip, take, term, cancellationToken);
        int total = await _appointmentDb.CountAllAsync(term, cancellationToken);

        return new PagedResponse<List<Appointment>>()
        {
            Result = all,
            Total = total
        };
    }

    public async Task<List<BranchSlotPosition>> GetBranchSlotsPositionsAsync(int branchId, CancellationToken cancellationToken = default)
    {
        return await _appointmentDb.GetBranchSlotsPositionsAsync(branchId, cancellationToken);
    }

    private static bool IsSlotUniqueViolation(DbUpdateException ex)
    {
        if (ex.InnerException is not PostgresException pgEx)
            return false;

        if (pgEx.SqlState != PostgresErrorCodes.UniqueViolation)
            return false;

        return pgEx.ConstraintName?.Contains("SlotId", StringComparison.OrdinalIgnoreCase) == true
               || pgEx.ConstraintName?.Contains("slot", StringComparison.OrdinalIgnoreCase) == true;
    }
}
