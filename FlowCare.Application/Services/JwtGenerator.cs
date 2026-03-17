using FlowCare.Application.Interfaces.Services_Interfaces;
using FlowCare.Application.settings;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProcurementLite.Application.Services
{
    public class JwtGenerator : IJwtGenerator
    {
        private readonly JwtSettings _jwtSettings;

        public JwtGenerator(IOptions<JwtSettings> jwtSettings)
        {
            _jwtSettings = jwtSettings.Value;
        }

        public string GenerateToken(List<Claim> claims)
        {
            if (string.IsNullOrWhiteSpace(_jwtSettings.ToeknKey))
                throw new InvalidOperationException("JWT access token key is not configured.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.ToeknKey));

            return GenerateJwtToken(key, claims, _jwtSettings.ExpirationInMinutes);
        }

        public string GenerateRefreshToken(List<Claim> claims)
        {
            claims.Add(new Claim("token_type", "refresh"));

            if (string.IsNullOrWhiteSpace(_jwtSettings.ToeknRefreshKey))
                throw new InvalidOperationException("JWT refresh token key is not configured.");

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.ToeknRefreshKey));

            // Refresh token expires in configurable days
            return GenerateJwtToken(key, claims, 60 * 24 * _jwtSettings.RefreshExpirationInDays);
        }

        private string GenerateJwtToken(SymmetricSecurityKey? key, List<Claim> claims, int expirationInMinutes)
        {
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expirationInMinutes),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public int? ValidateRefreshToken(string refreshToken)
        {
            if (string.IsNullOrWhiteSpace(refreshToken))
                return null;

            if (string.IsNullOrWhiteSpace(_jwtSettings.ToeknRefreshKey))
                return null;

            var handler = new JwtSecurityTokenHandler();
            var keyBytes = Encoding.UTF8.GetBytes(_jwtSettings.ToeknRefreshKey);

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(keyBytes),
                ValidateIssuer = !string.IsNullOrWhiteSpace(_jwtSettings.Issuer),
                ValidIssuer = _jwtSettings.Issuer,
                ValidateAudience = !string.IsNullOrWhiteSpace(_jwtSettings.Audience),
                ValidAudience = _jwtSettings.Audience,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.FromSeconds(30)
            };

            try
            {
                var principal = handler.ValidateToken(refreshToken, validationParameters, out var validatedToken);

                if (validatedToken is not JwtSecurityToken jwtToken ||
                    !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    return null;
                }

                var tokenType = principal.Claims.FirstOrDefault(c => c.Type == "token_type")?.Value;
                if (tokenType != "refresh")
                    return null;

                var nameId = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                if (!int.TryParse(nameId, out var userId))
                    return null;

                return userId;
            }
            catch (SecurityTokenExpiredException)
            {
                return null;
            }
            catch
            {
                return null;
            }
        }
    }
}