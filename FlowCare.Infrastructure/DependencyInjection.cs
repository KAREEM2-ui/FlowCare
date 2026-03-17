using FlowCare.Application.Interfaces.Persistence;
using FlowCare.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace FlowCare.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services,string ConnString,bool IsDevelopment)
    {
        services.AddScoped<IBranchDb, BranchDb>();
        services.AddScoped<IServiceTypeDb, ServiceTypeDb>();
        services.AddScoped<IStaffDb, StaffDb>();
        services.AddScoped<ICustomerDb, CustomerDb>();
        services.AddScoped<IAppointmentDb, AppointmentDb>();
        services.AddScoped<IStaffServiceTypeDb, StaffServiceTypeDb>();
        services.AddScoped<ISlotDb, SlotDb>();
        services.AddScoped<IAuditLogDb, AuditLogDb>();


        services.AddDbContext<AppDbContext>((options) =>
        {

            options.UseNpgsql(ConnString,
                
                options => options.MigrationsAssembly("FlowCare.Infrastructure"));

            if (IsDevelopment)
            {
                options.EnableDetailedErrors();
                options.EnableSensitiveDataLogging().LogTo(Console.WriteLine, LogLevel.Information);

            }



        });



        return services;
    }
}
