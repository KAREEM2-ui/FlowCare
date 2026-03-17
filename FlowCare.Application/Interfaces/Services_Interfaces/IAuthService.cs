using FlowCare.Application.DTOs.Requests;

namespace FlowCare.Application.Interfaces.Services;


public interface IAuthService
{
    Task<bool> IsUsernameTakenAsync(string username, CancellationToken cancellationToken = default);
    Task<bool> IsEmailTakenAsync(string email, CancellationToken cancellationToken = default);
    Task<LoginResponse?> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default);
    Task<RefreshTokenResponse?> RefreshTokenAsync(int userId, string? refreshToken, CancellationToken cancellationToken = default);
    Task RegisterCustomerAsync(RegisterCustomerRequest request, CancellationToken cancellationToken = default);
}