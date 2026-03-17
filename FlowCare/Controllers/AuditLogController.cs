using FlowCare.Application.DTOs;
using FlowCare.Application.DTOs.Responses;
using FlowCare.Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text;

namespace FlowCare_presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AuditLogController : ControllerBase
{
    private readonly IAuditLogService _auditLogService;

    public AuditLogController(IAuditLogService auditLogService)
    {
        _auditLogService = auditLogService;
    }

    private int? GetBranchId()
    {
        var val = User.FindFirstValue("BranchId");
        return val is null ? null : int.Parse(val);
    }

    /// <summary>
    /// View all audit logs — Admin only
    /// </summary>
    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> GetAllAuditLogs([FromQuery] int skip = 0, [FromQuery] int take = 20)
    {
        var logs = await _auditLogService.GetAllPaginatedAsync(skip, take);
        return Ok(ApiResponse<List<AuditLogResponse>>.Ok(logs.ToResponse()));
    }

    /// <summary>
    /// Export all audit logs as CSV — Admin only
    /// </summary>
    [HttpGet("export/csv")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> ExportAuditLogsCsv()
    {
        var logs = await _auditLogService.GetAllPaginatedAsync(0, int.MaxValue);

        var sb = new StringBuilder();
        sb.AppendLine("Id,ActorId,ActorRole,ActionTypeId,EntityTypeId,EntityId,Timestamp,MetadataJson");

        foreach (var log in logs)
        {
            sb.AppendLine($"{log.Id},{log.ActorId},{log.Role.Title},{log.ActionTypeId},{log.EntityTypeId},{log.EntityId},{log.Timestamp:o},{log.MetadataJson}");
        }

        var bytes = Encoding.UTF8.GetBytes(sb.ToString());
        return File(bytes, "text/csv", "audit_logs.csv");
    }

    /// <summary>
    /// View audit logs for manager's branch — BranchManager
    /// </summary>
    [HttpGet("branch")]
    [Authorize(Roles = "BranchManager")]
    public async Task<IActionResult> GetBranchAuditLogs([FromQuery] int skip = 0, [FromQuery] int take = 20)
    {
        var branchId = GetBranchId();
        if (!branchId.HasValue)
            return StatusCode(403, ApiResponse<object>.Fail("Branch not assigned"));

        var logs = await _auditLogService.GetByBranchIdPaginatedAsync(branchId.Value, skip, take);
        return Ok(ApiResponse<List<AuditLogResponse>>.Ok(logs.ToResponse()));
    }
}
