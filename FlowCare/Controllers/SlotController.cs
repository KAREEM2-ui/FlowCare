using FlowCare.Application.DTOs;
using FlowCare.Application.DTOs.Requests;
using FlowCare.Application.DTOs.Responses;
using FlowCare.Application.Interfaces.Services;
using FlowCare.Application.Services;
using FlowCare.Domain.Entities;
using FlowCare.Domain.Enums;
using FlowCare.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FlowCare_presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SlotController : ControllerBase
{
    private readonly ISlotService _slotService;
    private readonly IAuditLogService _auditLogService;
    private readonly IAppAuthorizationService _appAuthorizationService;

    public SlotController(ISlotService slotService, IAuditLogService auditLogService, IAppAuthorizationService appAuthorizationService)
    {
        _slotService = slotService;
        _auditLogService = auditLogService;
        _appAuthorizationService = appAuthorizationService;
    }

    private int GetUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    private string GetRole() => User.FindFirstValue(ClaimTypes.Role)!;

    public static int SoftDeleteRetentionDays { get; set; } = 30;

    [HttpGet("available")]
    [AllowAnonymous]
    public async Task<IActionResult> ListAvailableSlots([FromQuery] int branchId, [FromQuery] int serviceTypeId, [FromQuery] DateOnly? date)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);


        var slots = await _slotService.GetAvailableSlotsByBranchAndServiceAsync(branchId, serviceTypeId, date);
        return Ok(ApiResponse<List<SlotResponse>>.Ok(slots.ToResponse()));
    }

    [HttpGet("{slotId:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetSlot(int slotId)
    {
        var slot = await _slotService.GetByIdAsync(slotId);
        if (slot is null)
            return NotFound(ApiResponse<object>.Fail("Slot not found"));

        return Ok(ApiResponse<SlotResponse>.Ok(slot.ToResponse()));
    }

  

    [HttpPost]
    [Authorize(Roles = "Admin,BranchManager")]
    public async Task<IActionResult> CreateSlot([FromBody] CreateSlotRequest request)
    {
        // Authorization Check - Assigned Branch 
        if (!_appAuthorizationService.IsAssignedBranch(User, request.BranchId))
            throw new UnauthorizedAccessException($"{GetRole()} is not assgined to The Branch ");





        var slot = new Slot
        {
            BranchId = request.BranchId,
            ServiceTypeId = request.ServiceTypeId,
            StaffId = request.StaffId,
            StartAt = request.StartAt,
            EndAt = request.EndAt,
            Capacity = request.Capacity
        };

        var created = await _slotService.CreateSlotAsync(slot);

        await _auditLogService.LogAsync(new AuditLog
        {
            ActorId = GetUserId(),
            RoleId = Convert.ToInt32(Enum.Parse<UserRole>(GetRole())),
            ActionTypeId = 5,
            EntityTypeId = 2,
            EntityId = created.Id,
            Timestamp = DateTimeOffset.UtcNow,
            MetadataJson = $"{{\"action\":\"CREATE_SLOT\",\"branchId\":{request.BranchId}}}"
        });

        return CreatedAtAction(nameof(GetSlot), new { slotId = created.Id },
            ApiResponse<SlotResponse>.Ok(created.ToResponse(), "Slot created"));
    }

    [HttpPost("BulkCreation")]
    [Authorize(Roles = "Admin,BranchManager")]
    public async Task<IActionResult> CreateBulkSlots([FromBody] BulkCreateSlotRequest request)
    {
        // Authorization Check - Assigned Branch 
        if (!_appAuthorizationService.IsAssignedBranch(User, request.Slots.Select(s=>s.BranchId).ToList()))
            throw new UnauthorizedAccessException($"{GetRole()} is not assgined to some Branches ");




        var slots = request.Slots.Select(s => new Slot
        {
            BranchId = s.BranchId,
            ServiceTypeId = s.ServiceTypeId,
            StaffId = s.StaffId,
            StartAt = s.StartAt,
            EndAt = s.EndAt,
            Capacity = s.Capacity
        }).ToList();

        await _slotService.CreateBulkAsync(slots);

        await _auditLogService.LogAsync(new AuditLog
        {
            ActorId = GetUserId(),
            RoleId = Convert.ToInt32(Enum.Parse<UserRole>(GetRole())),
            ActionTypeId = 5,
            EntityTypeId = 2,
            EntityId = 0,
            Timestamp = DateTimeOffset.UtcNow,
            MetadataJson = $"{{\"action\":\"BULK_CREATE_SLOTS\",\"count\":{slots.Count}}}"
        });

        return Ok(ApiResponse<object>.Ok(new { count = slots.Count }, $"{slots.Count} slots created"));
    }

    [HttpPut("{slotId:int}")]
    [Authorize(Roles = "Admin,BranchManager")]
    public async Task<IActionResult> UpdateSlot(int slotId, [FromBody] UpdateSlotRequest request,CancellationToken ct)
    {

        Slot? slot = await _slotService.GetByIdAsync(slotId);
        if(slot == null)
            return NotFound(ApiResponse<object>.Fail("Slot not found"));

        

        // Authorization Check - Assigned Branch 
        if (!_appAuthorizationService.IsAssignedBranch(User, slot.BranchId))
            throw new UnauthorizedAccessException($"{GetRole()} is not assgined to The Branch ");

      

        var updated = await _slotService.UpdateSlotAsync(slot,request,ct);

        await _auditLogService.LogAsync(new AuditLog
        {
            ActorId = GetUserId(),
            RoleId = Convert.ToInt32(Enum.Parse<UserRole>(GetRole())),
            ActionTypeId = 6,
            EntityTypeId = 2,
            EntityId = slotId,
            Timestamp = DateTimeOffset.UtcNow,
            MetadataJson = "{\"action\":\"UPDATE_SLOT\"}"
        });

        return Ok(ApiResponse<SlotResponse>.Ok(updated.ToResponse(), "Slot updated"));
    }

    [HttpDelete("{slotId:int}")]
    [Authorize(Roles = "Admin,BranchManager")]
    public async Task<IActionResult> SoftDeleteSlot(int slotId,CancellationToken ct)
    {
        Slot? slot = await _slotService.GetByIdAsync(slotId,ct);
        if (slot == null)
            return NotFound(ApiResponse<object>.Fail("Slot not found"));



        // Authorization Check - Assigned Branch 
        if (!_appAuthorizationService.IsAssignedBranch(User, slot.BranchId))
            throw new UnauthorizedAccessException($"{GetRole()} is not assgined to The Branch ");


        await _slotService.SoftDeleteAsync(slot,ct);

        await _auditLogService.LogAsync(new AuditLog
        {
            ActorId = GetUserId(),
            RoleId = Convert.ToInt32(Enum.Parse<UserRole>(GetRole())),
            ActionTypeId = 7,
            EntityTypeId = 2,
            EntityId = slotId,
            Timestamp = DateTimeOffset.UtcNow,
            MetadataJson = "{\"action\":\"SOFT_DELETE_SLOT\"}"
        });

        return Ok(ApiResponse<object>.Ok(null!, "Slot soft-deleted"));
    }

    [HttpPost("cleanup")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CleanupExpiredSlots(CancellationToken ct)
    {
        await _slotService.HardDeleteExpiredAsync(ct);

        await _auditLogService.LogAsync(new AuditLog
        {
            ActorId = GetUserId(),
            RoleId = Convert.ToInt32(UserRole.Admin),
            ActionTypeId = 10,
            EntityTypeId = 2,
            EntityId = 0,
            Timestamp = DateTimeOffset.UtcNow,
            MetadataJson = $"{{\"action\":\"HARD_DELETE_EXPIRED_SLOTS\",\"retentionDays\":{SoftDeleteRetentionDays}}}"
        });

        return Ok(ApiResponse<object>.Ok(
            new { retentionDays = SoftDeleteRetentionDays },
            "Expired soft-deleted slots cleaned up"));
    }
}
