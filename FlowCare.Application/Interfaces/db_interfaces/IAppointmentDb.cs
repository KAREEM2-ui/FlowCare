using FlowCare.Application.DTOs.Responses;
using FlowCare.Domain.Entities;
using FlowCare.Domain.Enums;

namespace FlowCare.Application.Interfaces.Persistence;

public interface IAppointmentDb
{
    Task<List<Appointment>> GetAppointmentsByDateDescByBranchIdPaginatedAsync(int branchId, int skip, int take, string? term = null, CancellationToken cancellationToken = default);
    Task<Appointment?> GetByIdAsync(int appointmentId, CancellationToken cancellationToken = default);
    Task<Appointment> CreateAsync(Appointment appointment, CancellationToken cancellationToken = default);
    Task<Appointment> UpdateStatusAsync(int appointmentId, AppointmentStatus newStatus, CancellationToken cancellationToken = default);
    Task<Appointment> UpdateSlotAsync(int appointmentId, int newSlotId, CancellationToken cancellationToken = default);
    Task<List<Appointment>> GetByCustomerIdPaginatedAsync(int customerId, int skip, int take, string? term = null, CancellationToken cancellationToken = default);
    Task<List<Appointment>> GetByStaffIdPaginatedAsync(int staffId, int skip, int take, string? term = null, CancellationToken cancellationToken = default);
    Task<List<Appointment>> GetAllPaginatedAsync(int skip, int take, string? term = null, CancellationToken cancellationToken = default);
    
    Task<int> CountAllAsync(string? term = null, CancellationToken ct = default);
    Task<int> CountAllByBranchIdAsync(int branchId, string? term = null, CancellationToken ct = default);
    Task<int> CountAllByStaffIdAsync(int staffId, string? term = null, CancellationToken ct = default);
    Task<int> CountAllByCustomerIdAsync(int customerId, string? term = null, CancellationToken ct = default);
    Task<List<BranchSlotPosition>> GetBranchSlotsPositionsAsync(int branchId, CancellationToken cancellationToken = default);
}
