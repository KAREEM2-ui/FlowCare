using FlowCare.Application.DTOs.Requests;
using FlowCare.Application.DTOs.Responses;
using FlowCare.Application.Interfaces.Persistence;
using FlowCare.Application.Interfaces.Services;
using FlowCare.Application.Interfaces.Services_Interfaces;
using FlowCare.Domain.Entities;
using FlowCare.Domain.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace FlowCare.Application.Services;

public sealed class AuthService : IAuthService
{
    private readonly IStaffDb _staffDb;
    private readonly ICustomerDb _customerDb;
    private readonly IJwtGenerator _jwtGenerator;
    private readonly IPasswordHasher<object> _PasswordHasher;
    private readonly ILogger<IAuthService> _logger;
    private readonly IFileStorageService _fileStorageService;

    public AuthService(
        IStaffDb staffDb,
        ICustomerDb customerDb,
        IJwtGenerator jwtGenerator,
        IPasswordHasher<object> PasswordHasher,
        ILogger<AuthService> logger,
        IFileStorageService fileStorageService)
    {
        _staffDb = staffDb;
        _customerDb = customerDb;
        _jwtGenerator = jwtGenerator;
        _PasswordHasher = PasswordHasher;
        _logger = logger;
        _fileStorageService = fileStorageService;
    }

    public async Task<bool> IsUsernameTakenAsync(string username, CancellationToken cancellationToken = default)
    {
        try
        {
            var staff = await _staffDb.GetByUsernameAsync(username, cancellationToken);
            if (staff is not null)
                return true;

            var customer = await _customerDb.GetByUsernameAsync(username, cancellationToken);
            return customer is not null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed validating username");
            throw new Exception("Failed validating username");
        }
    }

    public async Task<bool> IsEmailTakenAsync(string email, CancellationToken cancellationToken = default)
    {
        try
        {
            var staff = await _staffDb.GetByEmailAsync(email, cancellationToken);
            if (staff is not null)
                return true;

            var customer = await _customerDb.GetByEmailAsync(email, cancellationToken);
            return customer is not null;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed validating email");
            throw new Exception("Failed validating email");
        }
    }

    public async Task<LoginResponse?> LoginAsync(LoginRequest request, CancellationToken cancellationToken = default)
    {
        try
        {
            var staff = await _staffDb.GetByUsernameAsync(request.Username, cancellationToken);
            if (staff is not null)
            {
                var result = _PasswordHasher.VerifyHashedPassword(staff, staff.PasswordHash, request.Password);
                if (result != PasswordVerificationResult.Success)
                    return null;

                List<Claim> claims = _CreateStafClaims(staff);


                return CreateLoginResponse(claims);
            }

            var customer = await _customerDb.GetByUsernameAsync(request.Username, cancellationToken);
            if (customer is null)
                return null;

            var customerResult = _PasswordHasher.VerifyHashedPassword(customer, customer.PasswordHash, request.Password);
            if (customerResult != PasswordVerificationResult.Success)
                return null;

           
            var customerClaims = _CreateClientClaims(customer);

            return CreateLoginResponse(customerClaims);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Login failed for username={Username}", request.Username);
            throw new Exception("Login failed");
        }
    }

    public async Task<RefreshTokenResponse?> RefreshTokenAsync(int userId, string? refreshToken, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(refreshToken))
            return null;

        try
        {
            var tokenUserId = _jwtGenerator.ValidateRefreshToken(refreshToken);
            if (!tokenUserId.HasValue || tokenUserId.Value != userId)
                return null;

            var staff = await _staffDb.GetByIdAsync(userId, cancellationToken);
            if (staff is not null)
            {
                var claims = _CreateStafClaims(staff);
                var newAccessToken = _jwtGenerator.GenerateToken(claims);

                return new RefreshTokenResponse(newAccessToken);
            }

            var customer = await _customerDb.GetByIdAsync(userId, cancellationToken);
            if (customer is null)
                return null;

            var customerClaims = _CreateClientClaims(customer);
            var token = _jwtGenerator.GenerateToken(customerClaims);

            return new RefreshTokenResponse(token);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error validating refresh token for userId={UserId}", userId);
            throw;
        }
    }

    public async Task RegisterCustomerAsync(RegisterCustomerRequest request, CancellationToken cancellationToken = default)
    {


        // save id image and get the reference 
        string ImageRef = await _fileStorageService.StoreCustomerIdImageAsync(request.IdImage, cancellationToken);



        var customer = new Customer
        {
            Username = request.Username,
            FullName = request.FullName,
            Email = request.Email,
            Phone = request.Phone,
            PasswordHash = string.Empty,
            IsActive = true,
            IdRefImage = ImageRef,
        };



        
        customer.PasswordHash = _PasswordHasher.HashPassword(customer, request.Password);
        await _customerDb.CreateCustomerAsync(customer, cancellationToken);
        
    }


    private LoginResponse CreateLoginResponse(List<Claim> claims)
    {
        var token = _jwtGenerator.GenerateToken(claims);
        var refreshToken = _jwtGenerator.GenerateRefreshToken(claims);

        return new LoginResponse(token, refreshToken);
    }


    private List<Claim> _CreateStafClaims(Staff staff)
    {
        string role = ((UserRole)staff.RoleId).ToString();


        List< Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, staff.Id.ToString()),

                    new Claim(ClaimTypes.Role, role),
                    new Claim("BranchId", staff.BranchId.ToString())
                };

        return claims;
    }

    private List<Claim> _CreateClientClaims(Customer customer)
    {
        List<Claim> claims = new List<Claim>()
                {
                    new Claim(ClaimTypes.NameIdentifier, customer.Id.ToString()),
                    new Claim(ClaimTypes.Role, "Customer"),
                };

        return claims;
    }
}