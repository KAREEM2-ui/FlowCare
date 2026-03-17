using FlowCare.Application.DTOs.Requests;
using FlowCare.Application.DTOs.Responses;
using FlowCare.Application.Pagination;
using FlowCare.Domain.Entities;
using FlowCare.Domain.Enums;


namespace FlowCare.Application.Interfaces.Services;

public interface IAppointmentService
{
    Task<PagedResponse<List<Appointment>>> GetAppointmentsByDateDescByBranchIdPaginatedAsync(int branchId, int skip, int take, string? term, CancellationToken cancellationToken = default);
    Task<Appointment?> GetByIdAsync(int appointmentId, CancellationToken cancellationToken = default);
    Task<Appointment> BookAppointmentAsync(BookAppointmentRequest AppointmentRequest, int CustomerId ,CancellationToken cancellationToken = default);
    Task<Appointment> CancelAppointmentAsync(int appointmentId, int customerId, CancellationToken cancellationToken = default);
    Task<Appointment> RescheduleAppointmentAsync(int appointmentId, int customerId, int newSlotId, CancellationToken cancellationToken = default);
    
    Task<PagedResponse<List<Appointment>>> GetAppointmentHistoryByCustomerIdPaginatedAsync(int customerId, int skip, int take,string? term, CancellationToken cancellationToken = default);
    Task<PagedResponse<List<Appointment>>> GetAppointmentsByStaffIdPaginatedAsync(int staffId, int skip, int take, string? term,CancellationToken cancellationToken = default);
    
    Task<Appointment> UpdateAppointmentStatusAsync(int appointmentId, AppointmentStatus newStatus, CancellationToken cancellationToken = default);
    
    Task<PagedResponse<List<Appointment>>> GetAllAsyncPaginated(int skip,int take,string? term,CancellationToken cancellationToken = default);
    Task<List<BranchSlotPosition>> GetBranchSlotsPositionsAsync(int branchId, CancellationToken cancellationToken = default);
}
