using Acme.Sistemas.Atena.Api.Config.Security;
using Acme.Sistemas.Atena.Api.Endpoints;
using Acme.Sistemas.Atena.Api.Hosted;
using Acme.Sistemas.Atena.Api.Middlewares;
using Acme.Sistemas.Core;
using Acme.Sistemas.Core.Const;
using Acme.Sistemas.Core.Security;
using Acme.Sistemas.Core.Settings;
using Acme.Sistemas.Domain.Entities.Auditoria;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.ExternalIntegration;
using Acme.Sistemas.Infrastructure;
using Acme.Sistemas.Infrastructure.Databases.Configuration;
using Acme.Sistemas.Infrastructure.Databases.Migrations;
using Acme.Sistemas.Infrastructure.Databases.Migrations.Configuration;
using Acme.Sistemas.Repository;
using Acme.Sistemas.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.RateLimiting;
using Microsoft.Extensions.Logging;

JwtSecurityTokenHandler.DefaultMapInboundClaims = false;

var builder = WebApplication.CreateBuilder(args);

// Feature flags: arquivo dedicado, hot-reload nativo via IConfiguration + IOptionsMonitor.
builder.Configuration.AddJsonFile("featureflags.json", optional: true, reloadOnChange: true);

builder.Services.Configure<FeatureFlagSettings>(builder.Configuration.GetSection(FeatureFlagSettings.SectionName));

builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection(JwtOptions.SectionName));
builder.Services.Configure<PublicAppOptions>(builder.Configuration.GetSection(PublicAppOptions.SectionName));
builder.Services.Configure<AdminOptions>(builder.Configuration.GetSection(AdminOptions.SectionName));
builder.Services.Configure<SeedOptions>(builder.Configuration.GetSection(SeedOptions.SectionName));

builder.Services.AddAcmeSecurity();
builder.Services.AddAcmeServices(builder.Configuration);
builder.Services.AddAcmeRepositories();
builder.Services.AddAcmeInfrastructure(builder.Configuration);
builder.Services.AddAcmeExternalIntegration();
builder.Services.AddEndpoints(typeof(Program).Assembly);

builder.Services.ConfigureHttpJsonOptions(opt =>
{
    opt.SerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<HttpTenantContextAccessor>();
builder.Services.AddScoped<ITenantContext>(sp => sp.GetRequiredService<HttpTenantContextAccessor>());
builder.Services.AddScoped<IMutableTenantContext>(sp => sp.GetRequiredService<HttpTenantContextAccessor>());
builder.Services.AddSingleton<IAuthorizationHandler, PermissaoAuthorizationHandler>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    // Blueprint Acme: "uma pasta por verbo" cria várias classes com o mesmo nome curto
    // (Request, Response, EnderecoRequest, ItemRequest...) em namespaces diferentes.
    // FullName como schemaId evita colisão sem precisar renomear cada DTO.
    options.CustomSchemaIds(t => t.FullName?.Replace('+', '.'));
});

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
builder.Services.AddHostedService<Acme.Sistemas.Atena.Api.Hosted.NFeTransmissaoWorker>();
builder.Services.AddHostedService<Acme.Sistemas.Atena.Api.Hosted.CertificadoVencimentoVarreduraWorker>();
builder.Services.AddHostedService<Acme.Sistemas.Atena.Api.Hosted.EmailDispatcherHostedService>();
builder.Services.AddHostedService<Acme.Sistemas.Atena.Api.Hosted.CacheCleanupWorker>();
builder.Services.AddHostedService<Acme.Sistemas.Atena.Api.Hosted.RecorrenciaFinanceiraWorker>();
builder.Services.AddHostedService<Acme.Sistemas.Atena.Api.Hosted.JobVerificarIntegridadePontoWorker>();
builder.Services.AddHostedService<Acme.Sistemas.Atena.Api.Hosted.JobAuditarGapsNsrWorker>();

// Bootstrap de tenant demo: somente em Development (proteção dupla com a flag Seed:AutoBootstrap).
if (builder.Environment.IsDevelopment())
    builder.Services.AddHostedService<Acme.Sistemas.Atena.Api.Hosted.DevTenantBootstrapHostedService>();

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
using (var scope = app.Services.CreateScope())
{
    var dataConfiguration = scope.ServiceProvider.GetRequiredService<IDataConfiguration>();
    var migrations = new List<IMigration>
        {
            new V20260101001_CriarTabelasTenant(),
            new V20260101002_AdicionarTenantIdTabelasExistentes(),
            new V20260101003_CriarTabelasRbac(),
            new V20260101004_CriarTabelaUsuarios(),
            new V20260101005_CriarTabelasDespesaReceita(),
            new V20260101006_CriarTabelaFechamentoPeriodo(),
            new V20260101007_CriarTabelaEmpresas(),
            new V20260101008_AdicionarConfirmacaoEmailUsuario(),
            new V20260101009_CriarTabelasFinanceiroFase2(),
            new V20260101010_CriarTabelaCentroDeCusto(),
            new V20260101011_CriarTabelasCadastros(),
            new V20260101012_CriarTabelasProdutos(),
            new V20260101013_CriarTabelasEstoque(),
            new V20260101014_AdicionarFifoEstoque(),
            new V20260101015_CriarTabelasCompras(),
            new V20260101016_CriarTabelasVendas(),
            new V20260101017_CriarTabelasFiscalNFe(),
            new V20260101018_CriarTabelasAuditoria(),
            new V20260510001_CriarTabelaNFeNumeracao()
        };

    var runner = new MigrationRunner(dataConfiguration, new Logger<MigrationRunner>(new LoggerFactory()));
    await runner.RunAsync(typeof(V20260101001_CriarTabelasTenant).Assembly);
}

app.UseCors();
app.UseRateLimiter();
app.UseAdminIpAllowlist();
app.UseAuthentication();
app.UseAuthorization();
app.UseTenantContext();
app.UseApiRequestAudit();

app.MapEndpoints();

app.MapGet("/health", () => Results.Ok(new { status = "ok", timestamp = DateTime.UtcNow }))
   .WithTags("Health");

app.Run();

public partial class Program;
