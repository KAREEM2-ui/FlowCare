namespace FlowCare.Application.settings
{
    public class JwtSettings
    {
        public string ToeknKey { get; set; } = null!;
        public string ToeknRefreshKey { get; set; } = null!;
        public string Issuer { get; set; } = null!;
        public string Audience { get; set; } = null!;
        public int ExpirationInMinutes { get; set; } = 60;
        public int RefreshExpirationInDays { get; set; } = 7;
    }
}