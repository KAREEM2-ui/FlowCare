using FlowCare.Application.Interfaces.Services;
using FlowCare.Domain.Entities;
using FlowCare.Domain.Enums;
using System.Security.Claims;

namespace FlowCare.Application.Services;

public class AppAuthorizationService : IAppAuthorizationService
{
    public bool IsAssignedBranch(ClaimsPrincipal? user, int branchId)
    {
        if (user is null) return false;

        // Admins and Customers bypass branch restriction
        if (!user.IsInRole(nameof(UserRole.BranchManager)) && !user.IsInRole(nameof(UserRole.Staff)))
            return true;

        // For Staff and BranchManager, verify the branch matches
        var branchClaim = user.FindFirst("BranchId")?.Value;
        if (int.TryParse(branchClaim, out var userBranchId))
        {
            return userBranchId == branchId;
        }

        return false;
    }

    public bool IsCustomerOwner(ClaimsPrincipal? user, Appointment appointment)
    {
        if (user is null) return false;

        // Admins, Managers, and Staff can access any customer's data
        if (!user.IsInRole(nameof(UserRole.Customer)))
            return true;

        // If the user is a customer, verify their ID matches the requested data
        var idClaim = user.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (int.TryParse(idClaim, out var currentUserId))
        {
            return currentUserId == appointment.CustomerId;
        }

        return false;
    }

    

    public bool IsAssignedBranch(ClaimsPrincipal? user, Appointment appointment)
    {
        if (appointment == null) return false;

        var branchId = user!.FindFirst("BranchId")?.Value;
        return branchId == appointment.BranchId.ToString();
    }

    public bool IsAssignedBranch(ClaimsPrincipal? user, List<int> BranchIds)
    {
        if (BranchIds.Count < 0) return false;

        var UserbranchId = user!.FindFirst("BranchId")?.Value;

        foreach (var branchId in BranchIds)
        {
            if(UserbranchId != branchId.ToString())
            {
                return false;
            }
        }

        return true;
    }
}