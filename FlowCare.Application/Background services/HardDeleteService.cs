using FlowCare.Application.Interfaces.Services;
using FlowCare.Domain.Entities;
using FlowCare.Domain.Enums;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace FlowCare.Infrastructure.Services;

public sealed class SlotCleanupCronHostedService : BackgroundService
{
    private static readonly TimeSpan CleanupInterval = TimeSpan.FromHours(24);

    private readonly IServiceScopeFactory _scopeFactory;
    private readonly ILogger<SlotCleanupCronHostedService> _logger;

    public SlotCleanupCronHostedService(
        IServiceScopeFactory scopeFactory,
        ILogger<SlotCleanupCronHostedService> logger)
    {
        _scopeFactory = scopeFactory;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await ExecuteCleanupAsync(stoppingToken);

        using var timer = new PeriodicTimer(CleanupInterval);

        while (await timer.WaitForNextTickAsync(stoppingToken))
        {
            await ExecuteCleanupAsync(stoppingToken);
        }
    }

    private async Task ExecuteCleanupAsync(CancellationToken cancellationToken)
    {
        try
        {
            using var scope = _scopeFactory.CreateScope();
            var slotService = scope.ServiceProvider.GetRequiredService<ISlotService>();
            var auditLogService = scope.ServiceProvider.GetRequiredService<IAuditLogService>();

                var deletedSlots = await slotService.HardDeleteExpiredAsync(cancellationToken);

            foreach (var slot in deletedSlots)
            {
                await auditLogService.LogAsync(new AuditLog
                {
                    ActorId = 0,
                    RoleId = (int)UserRole.Admin,
                    ActionTypeId = (int)AuditActionType.HardDeleteExpiredSlots,
                    EntityTypeId = (int)AuditEntityType.Slot,
                    EntityId = slot.Id,
                    Timestamp = DateTimeOffset.UtcNow,
                    MetadataJson = $"{{\"action\":\"HARD_DELETE_EXPIRED_SLOT\",\"branchId\":{slot.BranchId}}}"
                }, cancellationToken);
            }

            _logger.LogInformation("Slot cleanup executed. Deleted slots count: {DeletedCount}", deletedSlots.Count);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while running slot cleanup job.");
        }
    }
}