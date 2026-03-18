using FlowCare.Application.Interfaces.Services;
using FlowCare.Application.Interfaces.Services_Interfaces;
using FlowCare.Application.Services;
using FlowCare.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FlowCare_presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class FileController : ControllerBase
{
    private readonly IFileStorageService _fileStorageService;
    private readonly IAppointmentService _appointmentService; 
    private readonly ICustomerService _customerService;
    private readonly IAppAuthorizationService _appAuthorizationService;

    public FileController(
        IFileStorageService fileStorageService, 
        IAppointmentService appointmentService,
        ICustomerService customerService,
        IAppAuthorizationService appAuthorizationService)
    {
        _fileStorageService = fileStorageService;
        _appointmentService = appointmentService;
        _customerService = customerService;
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
    /// Retrieve customer ID image (Admins only)
    /// </summary>
    [HttpGet("customer/{customerId:int}")]
    [Authorize(Roles = nameof(UserRole.Admin))]
    public async Task<IActionResult> GetCustomerIdImage([FromRoute] int customerId, CancellationToken ct)
    {
        var customer = await _customerService.GetByIdAsync(customerId, ct);

        if (customer == null)
            return NotFound("Customer not found.");

        if (string.IsNullOrWhiteSpace(customer.IdRefImage))
            return NotFound("This customer does not have an ID image reference.");

        try
        {
            var (fileStream, contentType, fileName) =  _fileStorageService.RetrieveCustomerIdImageAsync(customer.IdRefImage);
            return File(fileStream, contentType, fileName);
        }
        catch (FileNotFoundException)
        {
            return NotFound("The requested customer ID image was not found on the server.");
        }
    }

    /// <summary>
    /// Retrieve appointment attachment (staff and above, or the customer if he/she created it)
    /// </summary>
    [HttpGet("appointment/{appointmentId:int}")]
    [Authorize(Roles = "Admin,BranchManager,Staff,Customer")]

    public async Task<IActionResult> GetAppointmentAttachment([FromRoute] int appointmentId, CancellationToken ct)
    {
        var appointment = await _appointmentService.GetByIdAsync(appointmentId, ct);

        if (appointment == null)
            return NotFound("Appointment not found.");
            
        if (string.IsNullOrWhiteSpace(appointment.AttachementReference))
            return NotFound("This appointment does not have an attachment.");

        // Authorization Check - Assigned Branch 
        if (!_appAuthorizationService.IsAssignedBranch(User,appointment))
            throw new UnauthorizedAccessException($"{GetRole()} is not assgined to The Branch ");


        // created by customer
        if(!_appAuthorizationService.IsCustomerOwner(User,appointment))
            throw new UnauthorizedAccessException($"{GetRole()} is not the Owner ");



       
         var (fileStream, contentType, fileName) = _fileStorageService.RetrieveAppointmentAttachmentAsync(appointment.AttachementReference);
         return File(fileStream, contentType, fileName);
       
    }
}