using FlowCare.Application.DTOs;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace FlowCare_presentation.middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                var traceId = context.TraceIdentifier;
                _logger.LogError(ex, "Exception with TraceId {TraceId}, Message: {Message}", traceId, ex.Message);

                context.Response.ContentType = "application/json";

                // 1. Map exception type to HTTP Status Code
                context.Response.StatusCode = ex switch
                {
                    KeyNotFoundException => 404,
                    
                    ValidationException => 400,
                    ArgumentException => 400,
                    InvalidOperationException => 400,

                    DbUpdateConcurrencyException => 409, // Conflict
                    DbUpdateException => 400,            // Bad Request
                    FileNotFoundException => 404,
                    UnauthorizedAccessException => 401,  // Forbidden
                    
                    _ => 500
                };

                // 2. Safely resolve the message to reveal to the client
                var message = ex switch
                {
                    // EF Core database exceptions: Never expose these! Provide a generic safe message.
                    DbUpdateConcurrencyException => "The record was modified by another user. Please refresh and try again.",
                    DbUpdateException => "A database constraint violation occurred (e.g., duplicate data).",
                    
                    // Domain exceptions: Generally safe to expose because you hardcoded them (e.g., "Appointment not found")
                    KeyNotFoundException => ex.Message,
                    ValidationException => ex.Message,
                    ArgumentException => ex.Message,
                    InvalidOperationException => ex.Message,
                    UnauthorizedAccessException => ex.Message,
                    FileNotFoundException => ex.Message,

                    // Everything else (500s, NullReferenceException, etc.): 
                    _ => "An internal server error occurred. Please contact support."
                };

                // 3. Create the standardized API response
                var apiResponse = ApiResponse<object>.Fail(
                    message: message,
                    errors: null,
                    traceId: traceId
                );

                var options = new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                var result = JsonSerializer.Serialize(apiResponse, options);

                await context.Response.WriteAsync(result);
            }
        }
    }
}
