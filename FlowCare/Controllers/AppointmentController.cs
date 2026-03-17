using FlowCare.Application.DTOs;
using FlowCare.Application.DTOs.Requests;
using FlowCare.Application.DTOs.Responses;
using FlowCare.Application.Interfaces.Services;
using FlowCare.Application.Pagination;
using FlowCare.Domain.Entities;
using FlowCare.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FlowCare_presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AppointmentController : ControllerBase
{
    private readonly IAppointmentService _appointmentService;
    private readonly IAuditLogService _auditLogService;
    private readonly IAppAuthorizationService _appAuthorizationService;

    public AppointmentController(IAppointmentService appointmentService, IAuditLogService auditLogService,IAppAuthorizationService appAuthorizationService)
    {
        _appointmentService = appointmentService;
        _auditLogService = auditLogService;
        _appAuthorizationService = appAuthorizationService;
    }

    private int GetUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    private string GetRole() => User.FindFirstValue(ClaimTypes.Role)!;
    private int? GetBranchId()
    {
        var val = User.FindFirstValue("BranchId");
        return val is null ? null : int.Parse(val);
    }

    // ───── Staff / Manager / Admin ─────

    /// <summary>
    /// List appointments — Admin (all branches, must pass branchId)
    /// </summary>      
    [HttpGet("admin")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ListAllAppointments([FromQuery] PaginatedRequest paginatedRequest,CancellationToken ct,[FromQuery] string? term = null)
    {

        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var all = await _appointmentService.GetAllAsyncPaginated(paginatedRequest.skip, paginatedRequest.take,term,ct);
        
        // Use ToResponse extension method you've setup previously on the DTO model mappings
        return Ok(ApiResponse<PagedResponse<List<AppointmentResponse>>>.Ok(new PagedResponse<List<AppointmentResponse>> 
        { 
            Result = all.Result.ToResponse(), 
            Total = all.Total 
        }));
    }

    /// <summary>
    /// List appointments — BranchManager (own branch)  / Admin
    /// </summary>
    [HttpGet("branch/{branchId:int}")]
    [Authorize(Roles = "Admin,BranchManager")]
    public async Task<IActionResult> ListAppointmentsBranch([FromRoute] int branchId,[FromQuery] PaginatedRequest paginatedRequest,CancellationToken ct,[FromQuery] string? term = null)
    {
        if(!ModelState.IsValid)
            return BadRequest(ModelState);


        if (!_appAuthorizationService.IsAssignedBranch(User, branchId))
            throw new UnauthorizedAccessException($"{GetRole()} is not assgined to The Branch ");







        var branchAppts = await _appointmentService.GetAppointmentsByDateDescByBranchIdPaginatedAsync(branchId, paginatedRequest.skip, paginatedRequest.take,term,ct);

        return Ok(ApiResponse<PagedResponse<List<Appointment>>>.Ok(branchAppts));
    }

    /// <summary>
    /// List appointments — Staff (assigned to me)
    /// </summary>
    [HttpGet("staff")]
    [Authorize(Roles = "Staff")]
    public async Task<IActionResult> ListAppointmentsStaff(CancellationToken ct, [FromQuery] int skip = 0, [FromQuery] int take = 20,[FromQuery] string? term = null)
    {
        var staffId = GetUserId();
        var staffAppts = await _appointmentService.GetAppointmentsByStaffIdPaginatedAsync(staffId, skip, take,term,ct);
        return Ok(ApiResponse<PagedResponse<List<AppointmentResponse>>>.Ok(new PagedResponse<List<AppointmentResponse>> 
        { 
            Result = staffAppts.Result.ToResponse(), 
            Total = staffAppts.Total 
        }));
    }



 
    // ───── Customer ─────

    /// <summary>
    /// Book appointment — Customer only
    /// </summary>
    [HttpPost]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> BookAppointment([FromForm] BookAppointmentRequest request)
    {
        var customerId = GetUserId();

        
        var created = await _appointmentService.BookAppointmentAsync(request,customerId);

        await _auditLogService.LogAsync(new AuditLog
        {
            ActorId = customerId,
            RoleId = Convert.ToInt32(UserRole.Customer),
            ActionTypeId = Convert.ToInt32(AuditActionType.BookAppointment),
            EntityTypeId = Convert.ToInt32(AuditEntityType.Appointment),
            EntityId = created.Id,
            Timestamp = DateTimeOffset.UtcNow,
            MetadataJson = $"{{\"action\":\"BOOK_APPOINTMENT\",\"slotId\":{request.SlotId}}}"
        });

        return CreatedAtAction(nameof(BookAppointment), new { appointmentId = created.Id },
            ApiResponse<AppointmentResponse>.Ok(created.ToResponse(), "Appointment booked"));
    }

    /// <summary>
    /// List my appointments — Customer only
    /// </summary>
    [HttpGet("my")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> ListMyAppointments(CancellationToken ct, [FromQuery] int skip = 0, [FromQuery] int take = 20, [FromQuery] string? term = null)
    {
        if(!ModelState.IsValid)
            return BadRequest(ModelState);

        var customerId = GetUserId();
        var appointments = await _appointmentService.GetAppointmentHistoryByCustomerIdPaginatedAsync(customerId, skip, take,term,ct);
        return Ok(ApiResponse<PagedResponse<List<AppointmentResponse>>>.Ok(new PagedResponse<List<AppointmentResponse>> 
        { 
            Result = appointments.Result.ToResponse(), 
            Total = appointments.Total 
        }));
    }

    /// <summary>
    /// Cancel my appointment — Customer only
    /// </summary>
    [HttpPatch("{appointmentId:int}/cancel")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> CancelMyAppointment(int appointmentId)
    {
        var customerId = GetUserId();
        var cancelled = await _appointmentService.CancelAppointmentAsync(appointmentId, customerId);

        await _auditLogService.LogAsync(new AuditLog
        {
            ActorId = customerId,
            RoleId = Convert.ToInt32(UserRole.Customer),
            ActionTypeId = Convert.ToInt32(AuditActionType.CancelAppointment),
            EntityTypeId = Convert.ToInt32(AuditEntityType.Appointment),
            EntityId = appointmentId,
            Timestamp = DateTimeOffset.UtcNow,
            MetadataJson = "{\"action\":\"CANCEL_APPOINTMENT\"}"
        });

        return Ok(ApiResponse<AppointmentResponse>.Ok(cancelled.ToResponse(), "Appointment cancelled"));
    }

    /// <summary>
    /// Reschedule my appointment — Customer only
    /// </summary>
    [HttpPut("{appointmentId:int}/reschedule")]
    [Authorize(Roles = "Customer")]
    public async Task<IActionResult> RescheduleMyAppointment(int appointmentId, [FromBody] RescheduleAppointmentRequest request)
    {
        var customerId = GetUserId();
        var rescheduled = await _appointmentService.RescheduleAppointmentAsync(appointmentId, customerId, request.NewSlotId);

        await _auditLogService.LogAsync(new AuditLog
        {
            ActorId = customerId,
            RoleId = Convert.ToInt32(UserRole.Staff),
            ActionTypeId = 3,
            EntityTypeId = 1,
            EntityId = appointmentId,
            Timestamp = DateTimeOffset.UtcNow,
            MetadataJson = $"{{\"action\":\"RESCHEDULE_APPOINTMENT\",\"newSlotId\":{request.NewSlotId}}}"
        });

        return Ok(ApiResponse<AppointmentResponse>.Ok(rescheduled.ToResponse(), "Appointment rescheduled"));
    }

    // ───── Staff / Manager / Admin ─────

    /// <summary>
    /// Update appointment status (checked-in, no-show, completed)
    /// </summary>
    [HttpPut("{appointmentId:int}/status")]
    [Authorize(Roles = "Staff")]
    public async Task<IActionResult> UpdateStatus(int appointmentId, [FromBody] UpdateAppointmentStatusRequest request)
    {
        var updated = await _appointmentService.UpdateAppointmentStatusAsync(appointmentId, request.Status);

        await _auditLogService.LogAsync(new AuditLog
        {
            ActorId = GetUserId(),
            RoleId = Convert.ToInt32(Enum.Parse<UserRole>(GetRole())),
            ActionTypeId = 4,
            EntityTypeId = 1,
            EntityId = appointmentId,
            Timestamp = DateTimeOffset.UtcNow,
            MetadataJson = $"{{\"action\":\"UPDATE_APPOINTMENT_STATUS\",\"newStatus\":\"{request.Status}\"}}"
        });

        return Ok(ApiResponse<AppointmentResponse>.Ok(updated.ToResponse(), "Status updated"));
    }

    /// <summary>
    /// Public queue positions for today's branch slots
    /// </summary>
    [HttpGet("branch/{branchId:int}/queue-positions")]
    [EndpointDescription("Public queue positions for today's branch slots")]
    [AllowAnonymous]
    public async Task<IActionResult> GetBranchQueuePositions([FromRoute] int branchId, CancellationToken ct)
    {
        List<BranchSlotPosition> Positons = await _appointmentService.GetBranchSlotsPositionsAsync(branchId, ct);
        return Ok(ApiResponse<List<BranchSlotPosition>>.Ok(Positons));
    }
}
