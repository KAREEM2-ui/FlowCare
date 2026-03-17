using System.Security.Claims;

namespace FlowCare.Application.Interfaces.Services_Interfaces
{
    public interface IJwtGenerator
    {
        string GenerateToken(List<Claim> claims);
        string GenerateRefreshToken(List<Claim> claims);

        // Validate refresh JWT and return the user id if valid, otherwise null
        int? ValidateRefreshToken(string refreshToken);
    }
}
