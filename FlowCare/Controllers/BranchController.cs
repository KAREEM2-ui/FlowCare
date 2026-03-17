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
public class BranchController : ControllerBase
{
    private readonly IBranchService _branchService;
    private readonly IServiceTypeService _serviceTypeService;
    private readonly IAuditLogService _auditLogService;

    public BranchController(IBranchService branchService, IServiceTypeService serviceTypeService, IAuditLogService auditLogService)
    {
        _branchService = branchService;
        _serviceTypeService = serviceTypeService;
        _auditLogService = auditLogService;
    }

    private int GetUserId() => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> ListBranches([FromQuery] int skip = 0, [FromQuery] int take = 20)
    {
        var branches = await _branchService.GetAllBranchesPaginatedAsync(skip, take);
        return Ok(ApiResponse<PagedResponse<List<BranchResponse>>>.Ok(new PagedResponse<List<BranchResponse>> { Result = branches.Result.ToResponse(), Total = branches.Total }));
    }

    [HttpGet("{branchId:int}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetBranch(int branchId)
    {
        var branch = await _branchService.GetBranchByIdAsync(branchId);
        if (branch is null)
            return NotFound(ApiResponse<object>.Fail("Branch not found"));

        return Ok(ApiResponse<BranchResponse>.Ok(branch.ToResponse()));
    }

    [HttpGet("{branchId:int}/services")]
    [AllowAnonymous]
    public async Task<IActionResult> ListServicesByBranch(int branchId)
    {
        var services = await _serviceTypeService.GetServicesByBranchIdAsync(branchId);
        return Ok(ApiResponse<List<ServiceTypeResponse>>.Ok(services.ToResponse()));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> CreateBranch([FromBody] CreateBranchRequest request)
    {
        var branch = new Branch
        {
            Name = request.Name,
            City = request.City,
            Address = request.Address,
            Timezone = request.Timezone
        };

        var created = await _branchService.CreateBranchAsync(branch);

        await _auditLogService.LogAsync(new AuditLog
        {
            ActorId = GetUserId(),
            RoleId = Convert.ToInt32(UserRole.Admin),
            ActionTypeId = (int)AuditActionType.CreateBranch,
            EntityTypeId = (int)AuditEntityType.Branch,
            EntityId = created.Id,
            Timestamp = DateTimeOffset.UtcNow,
            MetadataJson = $"{{\"action\":\"CREATE_BRANCH\",\"name\":\"{created.Name}\"}}"
        });

        return CreatedAtAction(nameof(GetBranch), new { branchId = created.Id },
            ApiResponse<BranchResponse>.Ok(created.ToResponse(), "Branch created"));
    }

    [HttpPut("{branchId:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateBranch(int branchId, [FromBody] UpdateBranchRequest request)
    {
        var branch = new Branch
        {
            Id = branchId,
            Name = request.Name,
            City = request.City,
            Address = request.Address,
            Timezone = request.Timezone
        };

        var updated = await _branchService.UpdateBranchAsync(branch);

        await _auditLogService.LogAsync(new AuditLog
        {
            ActorId = GetUserId(),
            RoleId = Convert.ToInt32(UserRole.Admin),
            ActionTypeId = (int)AuditActionType.UpdateBranch,
            EntityTypeId = (int)AuditEntityType.Branch,
            EntityId = branchId,
            Timestamp = DateTimeOffset.UtcNow,
            MetadataJson = "{\"action\":\"UPDATE_BRANCH\"}"
        });

        return Ok(ApiResponse<BranchResponse>.Ok(updated.ToResponse(), "Branch updated"));
    }
}
