using FlowCare.Application.DTOs;
using FlowCare.Application.DTOs.Responses;
using FlowCare.Application.Interfaces.Services;
using FlowCare.Application.Pagination;
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
[Authorize(Roles = "Admin,BranchManager")]
public class StaffController : ControllerBase
{
    private readonly IStaffService _staffService;
    private readonly IStaffServiceTypeService _staffServiceTypeService;
    private readonly IAuditLogService _auditLogService;
    private readonly IAppAuthorizationService _appAuthorizationService;

    public StaffController(IStaffService staffService, IStaffServiceTypeService staffServiceTypeService, IAuditLogService auditLogService, IAppAuthorizationService appAuthorizationService)
    {
        _staffService = staffService;
        _staffServiceTypeService = staffServiceTypeService;
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

    /// <summary>
    /// List all staff accross different branches. 
    /// </summary>
    [HttpGet]
    [Authorize(Roles = $"{nameof(UserRole.Admin)}")]
    public async Task<IActionResult> ListStaff(CancellationToken ct,[FromQuery] int skip = 0, [FromQuery] int take = 20,[FromQuery] string? term = null)
    {



        var all = await _staffService.GetAllStaffPaginatedAsync(skip, take,term,ct);

        var response = new PagedResponse<List<StaffResponse>>()
        {
            Result = all.Result.ToResponse(),
            Total = all.Total,
        };


        return Ok(ApiResponse<PagedResponse<List<StaffResponse>>>.Ok(response));



    }


    /// <summary>
    /// List staff of specfic branch 
    /// </summary>
    [HttpGet("{branchId:int}")]
    [Authorize(Roles = $"{nameof(UserRole.Admin)},{nameof(UserRole.BranchManager)}")]
    public async Task<IActionResult> ListStaff([FromRoute] int branchId,CancellationToken ct, [FromQuery] int skip = 0, [FromQuery] int take = 20, [FromQuery] string? term = null)
    {

        if (!_appAuthorizationService.IsAssignedBranch(User, branchId))
            throw new UnauthorizedAccessException($"{GetRole()} is not assgined to The Branch ");





        var all = await _staffService.GetStaffByBranchIdPaginatedAsync(branchId,skip, take,term,ct);

        var response = new PagedResponse<List<StaffResponse>>()
        {
            Result = all.Result.ToResponse(),
            Total = all.Total,
        };


        return Ok(ApiResponse<PagedResponse<List<StaffResponse>>>.Ok(response));



    }

    /// <summary>
    /// Assign staff to service type. Admin ? system-wide, Manager ? branch-only
    /// </summary>
    [HttpPost("{staffId:int}/assign-service/{serviceTypeId:int}")]
    public async Task<IActionResult> AssignStaffToService(int staffId, int serviceTypeId)
    {
        await _staffServiceTypeService.AssignAsync(staffId, serviceTypeId);

        await _auditLogService.LogAsync(new AuditLog
        {
            ActorId = GetUserId(),
            RoleId = Convert.ToInt32(GetRole()),
            ActionTypeId = (int)AuditActionType.AssignStaffService,
            EntityTypeId = (int)AuditEntityType.Staff,
            EntityId = staffId,
            Timestamp = DateTimeOffset.UtcNow,
            MetadataJson = $"{{\"action\":\"ASSIGN_STAFF_SERVICE\",\"serviceTypeId\":{serviceTypeId}}}"
        });

        return Ok(ApiResponse<object>.Ok(null!, "Staff assigned to service"));
    }

    /// <summary>
    /// Assign staff to branch. Admin ? system-wide, Manager ? branch-only
    /// </summary>
    [HttpPut("{staffId:int}/assign-branch/{branchId:int}")]
    public async Task<IActionResult> AssignStaffToBranch(int staffId, int branchId)
    {
        var updated = await _staffService.ChangeStaffBranchAsync(staffId, branchId);

        await _auditLogService.LogAsync(new AuditLog
        {
            ActorId = GetUserId(),
            RoleId = Convert.ToInt32(GetRole()),
            ActionTypeId = (int)AuditActionType.AssignStaffBranch,
            EntityTypeId = (int)AuditEntityType.Staff,
            EntityId = staffId,
            Timestamp = DateTimeOffset.UtcNow,
            MetadataJson = $"{{\"action\":\"ASSIGN_STAFF_BRANCH\",\"branchId\":{branchId}}}"
        });

        return Ok(ApiResponse<StaffResponse>.Ok(updated.ToResponse(), "Staff branch updated"));
    }

    [HttpGet("{staffId:int}/services")]
    public async Task<IActionResult> GetStaffServices(int staffId)
    {
        var services = await _staffServiceTypeService.GetServicesOfStaffByStaffIdAsync(staffId);
        return Ok(ApiResponse<List<ServiceTypeResponse>>.Ok(services.ToResponse()));
    }
}
