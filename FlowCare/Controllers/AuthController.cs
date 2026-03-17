using FlowCare.Application.DTOs;
using FlowCare.Application.DTOs.Requests;
using FlowCare.Application.DTOs.Responses;
using FlowCare.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlowCare_presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("register")]
    public async Task<ActionResult<ApiResponse<object>>> Register([FromForm] RegisterCustomerRequest request, CancellationToken ct) 
    {
        if (!ModelState.IsValid)
        {
            var errors = ModelState
                .Where(element => element.Value?.Errors.Count > 0)
                .ToDictionary(element => element.Key, element => element.Value);

            return BadRequest(ApiResponse<object>.Fail("Registration details are not correct", errors, HttpContext.TraceIdentifier));
        }

      
            
            if (await _authService.IsUsernameTakenAsync(request.Username, ct))
                return BadRequest(ApiResponse<object>.Fail("Username already taken",
                    new { Username = "Username already taken" }, HttpContext.TraceIdentifier));



            if (await _authService.IsEmailTakenAsync(request.Email, ct))
                return BadRequest(ApiResponse<object>.Fail("Email already taken",
                    new { Email = "Email already taken" }, HttpContext.TraceIdentifier));



            await _authService.RegisterCustomerAsync(request, ct);

            return Ok(ApiResponse<object>.Ok(null, "Customer account registered"));
        
        
    }

    [HttpPost("login")]
    public async Task<ActionResult<ApiResponse<LoginResponse>>> Login([FromBody] LoginRequest request, CancellationToken ct)
    {
        try
        {
            var loginResponse = await _authService.LoginAsync(request, ct);
            if (loginResponse is null)
            {
                return Unauthorized(ApiResponse<object>.Fail("Username or password are not correct",
                    traceId: HttpContext.TraceIdentifier));
            }

            var jwtCookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(15)
            };

            var refreshCookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Path = "/api/auth/refresh-token",
                Expires = DateTime.UtcNow.AddDays(7)
            };

            Response.Cookies.Append("Token", loginResponse.Token, jwtCookieOptions);
            Response.Cookies.Append("RefreshToken", loginResponse.RefreshToken, refreshCookieOptions);

            return Ok(ApiResponse<LoginResponse>.Ok(loginResponse, "Login successfully"));
        }
        catch
        {
            return StatusCode(500, ApiResponse<object>.Fail("Internal server error",
                new { server = "Internal Server Error" }, HttpContext.TraceIdentifier));
        }
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult<ApiResponse<RefreshTokenResponse>>> RefreshToken([FromBody] RefreshTokenRequest request, CancellationToken ct)
    {
        try
        {
            var refreshToken = await _authService.RefreshTokenAsync(request.UserId, Request.Cookies["RefreshToken"], ct);
            if (refreshToken is null)
            {
                return Unauthorized(ApiResponse<object>.Fail("Invalid refresh token",
                    traceId: HttpContext.TraceIdentifier));
            }

            var jwtCookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTime.UtcNow.AddMinutes(15)
            };

            Response.Cookies.Append("Token", refreshToken.Token, jwtCookieOptions);

            return Ok(ApiResponse<RefreshTokenResponse>.Ok(refreshToken, "Token generated successfully"));
        }
        catch
        {
            return StatusCode(500, ApiResponse<object>.Fail("Internal server error",
                new { server = "Internal Server Error" }, HttpContext.TraceIdentifier));
        }
    }

    [HttpPost("logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("RefreshToken");
        Response.Cookies.Delete("Token");

        return Ok(ApiResponse<object>.Ok(new { }, "Logged out successfully"));
    }
}