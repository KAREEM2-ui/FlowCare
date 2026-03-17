namespace FlowCare.Application.Interfaces.Services;

public interface ICustomerRateLimiter
{
    bool TryAcquireBooking(int customerId, DateTimeOffset nowUtc);
    bool TryAcquireReschedule(int customerId, DateTimeOffset nowUtc);
}