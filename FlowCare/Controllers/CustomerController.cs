using FlowCare.Application.DTOs;
using FlowCare.Application.DTOs.Responses;
using FlowCare.Application.Interfaces.Services;
using FlowCare.Application.Pagination;
using FlowCare.Domain.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FlowCare_presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize(Roles = "Admin,BranchManager")]
public class CustomerController : ControllerBase
{
    private readonly ICustomerService _customerService;

    public CustomerController(ICustomerService customerService)
    {
        _customerService = customerService;
    }



    [HttpGet]
    [Authorize(nameof(UserRole.Admin))]
    public async Task<IActionResult> ListCustomers(CancellationToken ct,[FromQuery] int skip = 0, [FromQuery] int take = 20, [FromQuery] string? term = null)
    {
     
          var all = await _customerService.GetAllCustomersPaginatedAsync(skip, take,term,ct);

        var response = new PagedResponse<List<CustomerResponse>>()
        {
            Result = all.Result.ToResponse(),
            Total = all.Total
        };
          return Ok(ApiResponse<PagedResponse<List<CustomerResponse>>>.Ok(response));
       

       
    }

    [HttpGet("{customerId:int}")]
    [Authorize(nameof(UserRole.Admin))]

    public async Task<IActionResult> GetCustomer(int customerId)
    {
        var customer = await _customerService.GetByIdAsync(customerId);
        if (customer is null)
            return NotFound(ApiResponse<object>.Fail("Customer not found"));

        return Ok(ApiResponse<CustomerResponse>.Ok(customer.ToResponse()));
    }
}
