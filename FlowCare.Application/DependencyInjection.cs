using FlowCare.Application.Interfaces.Services;
using FlowCare.Application.Interfaces.Services_Interfaces;
using FlowCare.Application.Services;
using FlowCare.Infrastructure.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using ProcurementLite.Application.Services;

namespace FlowCare.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IBranchService, BranchService>();
        services.AddScoped<IServiceTypeService, ServiceTypeService>();
        services.AddScoped<IStaffService, StaffService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IAppointmentService, AppointmentService>();
        services.AddScoped<IStaffServiceTypeService, StaffServiceTypeService>();
        services.AddScoped<ISlotService, SlotService>();
        services.AddScoped<IAuditLogService, AuditLogService>();
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IJwtGenerator, JwtGenerator>();
        services.AddScoped<IFileStorageService, LocalFileStorageService>();
        services.AddScoped<IAppAuthorizationService, AppAuthorizationService>();

        services.AddSingleton<ICustomerRateLimiter, CustomerRateLimiter>();

        services.AddSingleton<IPasswordHasher<object>, PasswordHasher<object>>();



        services.AddHostedService<SlotCleanupCronHostedService>();

        return services;
    }
}
