using System.Collections.Concurrent;
using FlowCare.Application.Interfaces.Services;
using FlowCare.Application.settings;
using Microsoft.Extensions.Options;

namespace FlowCare.Application.Services;

public sealed class CustomerRateLimiter : ICustomerRateLimiter
{
    private readonly CustomerRateLimiterSettings _settings;

    private readonly ConcurrentDictionary<int, PriorityQueue<DateTimeOffset, DateTimeOffset>> _bookingBuckets = new();
    private readonly ConcurrentDictionary<int, PriorityQueue<DateTimeOffset, DateTimeOffset>> _rescheduleBuckets = new();

    public CustomerRateLimiter(IOptions<CustomerRateLimiterSettings> settings)
    {
        _settings = settings.Value;
    }

    public bool TryAcquireBooking(int customerId, DateTimeOffset nowUtc)
    {
        return TryAcquire(
            _bookingBuckets,
            customerId,
            nowUtc,
            _settings.Booking.Enabled,
            _settings.Booking.Limit,
            TimeSpan.FromSeconds(_settings.Booking.TimeWindowInSeconds));
    }

    public bool TryAcquireReschedule(int customerId, DateTimeOffset nowUtc)
    {
        return TryAcquire(
            _rescheduleBuckets,
            customerId,
            nowUtc,
            _settings.Reschedule.Enabled,
            _settings.Reschedule.Limit,
            TimeSpan.FromSeconds(_settings.Reschedule.TimeWindowInSeconds));
    }

    private static bool TryAcquire(
        ConcurrentDictionary<int, PriorityQueue<DateTimeOffset, DateTimeOffset>> dictionary,
        int customerId,
        DateTimeOffset nowUtc,
        bool enabled,
        int limit,
        TimeSpan window)
    {
        if (!enabled)
            return true;

        if (limit <= 0)
            return false;

        var queue = dictionary.GetOrAdd(customerId, _ => new PriorityQueue<DateTimeOffset, DateTimeOffset>());
        var cutoff = nowUtc - window;

        while (queue.TryPeek(out var oldest, out _) && oldest < cutoff)
        {
            queue.Dequeue();
        }

        if (queue.Count >= limit)
        {
            return false;
        }

        queue.Enqueue(nowUtc, nowUtc);
        return true;
    }
}