using FlowCare.Application;
using FlowCare.Application.Interfaces.Services_Interfaces;
using FlowCare.Infrastructure;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using ProcurementLite.Application.Services;
using System.Text;
using FlowCare.Application.settings;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "My API", Version = "v1" });

    // Add JWT Authentication
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter JWT token like: Bearer eyJhbGciOi..."
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});



// services reegisteration in the DI 
string? connString = builder.Configuration.GetConnectionString("DefaultConnection");
if (connString == null)
    throw new Exception("Connection string is not provided");

builder.Services.AddInfrastructure(connString, builder.Environment.IsDevelopment());
builder.Services.AddApplication();

builder.Services.Configure<JwtSettings>(builder.Configuration.GetSection("Jwt"));





// auth jwt

var jwtSection = builder.Configuration.GetSection("Jwt").Get<JwtSettings>();
var jwtKey = jwtSection?.ToeknKey;
var jwtIssuer = jwtSection?.Issuer;
var jwtAudience = jwtSection?.Audience;

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = true;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey!)),
        ValidateIssuer = !string.IsNullOrWhiteSpace(jwtIssuer),
        ValidIssuer = jwtIssuer,
        ValidateAudience = !string.IsNullOrWhiteSpace(jwtAudience),
        ValidAudience = jwtAudience,
        ValidateLifetime = true,
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {

            context.Request.Cookies.TryGetValue("Token", out var token); // must match cookie name set by your login
            Console.WriteLine($"Cookie present:, Token: {token}");

            if (!string.IsNullOrEmpty(token))
            {
                context.Token = token;
            }

            return Task.CompletedTask;
        }
    };
});









    // to be activated when consuming the endpoints from browser client //

//// Csrf service registration
//builder.Services.Configure<CsrfSettings>(builder.Configuration.GetSection("csrf"));
//builder.Services.AddSingleton<CsrfService>();

// CORS policy
//builder.Services.AddCors(options =>
//{
//    options.AddPolicy("CorsPolicy", policy =>
//    {
//        policy
//            .WithOrigins("http://localhost:5173") // client address
//            .AllowAnyHeader()
//            .AllowAnyMethod()
//            .AllowCredentials(); 
//    });
//});

builder.Logging.ClearProviders();


builder.Logging.AddSimpleConsole(options =>
{
    options.IncludeScopes = true;
    options.SingleLine = false;
    options.TimestampFormat = "HH:mm:ss ";

});




var app = builder.Build();

app.UseMiddleware<FlowCare_presentation.middleware.GlobalExceptionMiddleware>();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "FlowCare v1");
        c.RoutePrefix = "swagger"; // use "" to serve at root "/"
    });
}


app.UseAuthentication();
app.UseAuthorization();


// middleware to inlcude the TraceId in the logs 
app.Use(async (context, next) =>
{
    using (context.RequestServices
        .GetRequiredService<ILoggerFactory>()
        .CreateLogger("RequestScope")
        .BeginScope(new Dictionary<string, object>
        {
            ["TraceId"] = context.TraceIdentifier
        }))
    {
        await next();
    }
});

app.MapControllers();

app.Run();
