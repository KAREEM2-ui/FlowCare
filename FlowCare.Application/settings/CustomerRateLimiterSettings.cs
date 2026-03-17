namespace FlowCare.Application.settings;

public sealed class CustomerRateLimiterSettings
{
    public ActionRateLimitSettings Booking { get; set; } = new();
    public ActionRateLimitSettings Reschedule { get; set; } = new();
}

public sealed class ActionRateLimitSettings
{
    public bool Enabled { get; set; } = true;
    public int Limit { get; set; } = 3;
    public int TimeWindowInSeconds { get; set; } = 86400; // 1 day
}   