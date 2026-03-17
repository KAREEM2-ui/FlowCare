

using FlowCare_presentation.validations;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
namespace FlowCare.Application.DTOs.Requests;


public sealed record LoginRequest(
    string Username,
    string Password);

public sealed record RegisterCustomerRequest(
    string Username,
    string Password,
    string FullName,
    string Email,
    string? Phone)
{
    [Required]
    [FileSizeValidator(2,5)]
    [FileTypeValidator(new[] { ".png", ".jpg", ".jpeg" })]
    public IFormFile IdImage { get; init; } = default!;
}

public sealed record RefreshTokenRequest(int UserId);