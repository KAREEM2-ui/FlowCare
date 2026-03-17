using System.Security.Claims;
using FlowCare.Domain.Entities;

namespace FlowCare.Application.Interfaces.Services;

public interface IAppAuthorizationService
{
    bool IsAssignedBranch(ClaimsPrincipal? user, int branchId);
    bool IsAssignedBranch(ClaimsPrincipal? user, List<int> BranchIds);


    bool IsCustomerOwner(ClaimsPrincipal? user, Appointment appointment);
    
    bool IsAssignedBranch(ClaimsPrincipal? user, Appointment appointment);
}