using System.IdentityModel.Tokens.Jwt;
using System.Text;
using Acme.Sistemas.Atena.Api.Config.Security;
using Acme.Sistemas.Atena.Api.Endpoints;
using Acme.Sistemas.Atena.Api.Hosted;
using Acme.Sistemas.Atena.Api.Middlewares;
using Acme.Sistemas.Core;
using Acme.Sistemas.Core.Const;
using Acme.Sistemas.Core.Security;
using Acme.Sistemas.Core.Settings;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.ExternalIntegration;
using Acme.Sistemas.Infrastructure;
using Acme.Sistemas.Repository;
using Acme.Sistemas.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using System.Threading.RateLimiting;

JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(JwtOptions.SectionName));
builder.Services.Configure<PublicAppOptions>(builder.Configuration.GetSection(PublicAppOptions.SectionName));

builder.Services.AddAcmeSecurity();
builder.Services.AddAcmeServices();
builder.Services.AddAcmeRepositories();
builder.Services.AddAcmeInfrastructure(builder.Configuration);
builder.Services.AddAcmeExternalIntegration();
builder.Services.AddEndpoints(typeof(Program).Assembly);

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<ITenantContext, HttpTenantContextAccessor>();
builder.Services.AddSingleton<IAuthorizationHandler, PermissaoAuthorizationHandler>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var jwt = builder.Configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>() ?? new JwtOptions();
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwt.Issuer,
            ValidateAudience = true,
            ValidAudience = jwt.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.SigningKey)),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.FromSeconds(30),
            NameClaimType = "sub",
            RoleClaimType = "role"
        };
        options.Events = JwtBlacklistEvents.Build();
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPermissionPolicies(Permissions.All());
});

builder.Services.AddHostedService<PermissionsSeedHostedService>();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin();
    });
});

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.AddPolicy("tenant-registration", httpContext =>
    {
        var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        return RateLimitPartition.GetFixedWindowLimiter(ip, _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = 3,
            Window = TimeSpan.FromHours(1),
            QueueLimit = 0,
            AutoReplenishment = true
        });
    });

    options.AddPolicy("email-confirmation", httpContext =>
    {
        var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        return RateLimitPartition.GetFixedWindowLimiter(ip, _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = 10,
            Window = TimeSpan.FromMinutes(10),
            QueueLimit = 0,
            AutoReplenishment = true
        });
    });

    options.AddPolicy("auth-login", httpContext =>
    {
        var ip = httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown";
        return RateLimitPartition.GetFixedWindowLimiter(ip, _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = 10,
            Window = TimeSpan.FromMinutes(1),
            QueueLimit = 0,
            AutoReplenishment = true
        });
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseRateLimiter();
app.UseAuthentication();
app.UseAuthorization();
app.UseTenantContext();

app.MapEndpoints();

app.MapGet("/health", () => Results.Ok(new { status = "ok", timestamp = DateTime.UtcNow }))
   .WithTags("Health");

app.Run();

public partial class Program;
