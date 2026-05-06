using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Messaging;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Core.Security;
using Acme.Sistemas.Core.Settings;
using Acme.Sistemas.Domain.Entities.Permissions;
using Acme.Sistemas.Domain.Entities.Tenants;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Microsoft.Extensions.Options;
using UsuarioEntity = Acme.Sistemas.Domain.Entities.Users.Usuario;

namespace Acme.Sistemas.Services.V1.Tenant.Command.CriarTenant;

public sealed class CriarTenantCommandHandler
    : IRequestHandler<CriarTenantCommand, ResponseDefault<CriarTenantCommandResult>>
{
    private static readonly TimeSpan ConfirmationTokenLifetime = TimeSpan.FromHours(24);

    private readonly ITenantRepository _tenants;
    private readonly IRoleRepository _roles;
    private readonly IPermissionRepository _permissions;
    private readonly IRolePermissionRepository _rolePermissions;
    private readonly IUsuarioRepository _usuarios;
    private readonly IUserRoleRepository _userRoles;
    private readonly IEmailQueueService _emails;
    private readonly PublicAppOptions _publicApp;

    public CriarTenantCommandHandler(
        ITenantRepository tenants,
        IRoleRepository roles,
        IPermissionRepository permissions,
        IRolePermissionRepository rolePermissions,
        IUsuarioRepository usuarios,
        IUserRoleRepository userRoles,
        IEmailQueueService emails,
        IOptions<PublicAppOptions> publicApp)
    {
        _tenants = tenants;
        _roles = roles;
        _permissions = permissions;
        _rolePermissions = rolePermissions;
        _usuarios = usuarios;
        _userRoles = userRoles;
        _emails = emails;
        _publicApp = publicApp.Value;
    }

    public async Task<ResponseDefault<CriarTenantCommandResult>> Handle(
        CriarTenantCommand request,
        CancellationToken cancellationToken)
    {
        var cnpjDigits = new string(request.Cnpj.Where(char.IsDigit).ToArray());
        var existing = await _tenants.GetByCnpjAsync(cnpjDigits, cancellationToken);
        if (existing is not null)
        {
            return ResponseDefault<CriarTenantCommandResult>.Conflict(
                "Já existe tenant cadastrado com este CNPJ.");
        }

        var tenant = new Domain.Entities.Tenants.Tenant
        {
            RazaoSocial = request.RazaoSocial,
            Cnpj = cnpjDigits,
            Plano = request.Plano,
            Status = StatusAtivo.Ativo,
            FusoHorario = request.FusoHorario ?? "America/Sao_Paulo",
            CorPrimaria = request.CorPrimaria,
            LogoUrl = request.LogoUrl,
            CreatedAt = DateTime.UtcNow
        };

        await _tenants.AddAsync(tenant, cancellationToken);

        var limites = BuildLimitesFor(tenant.Id, request.Plano);
        await _tenants.UpsertLimitesAsync(limites, cancellationToken);

        var adminRoleId = await SeedAdminRoleAsync(tenant.Id, cancellationToken);
        var adminUser = await SeedAdminUserAsync(tenant.Id, adminRoleId, request, cancellationToken);

        return ResponseDefault<CriarTenantCommandResult>.Created(
            new CriarTenantCommandResult(
                tenant.Id, tenant.RazaoSocial, tenant.Cnpj, tenant.Plano,
                adminUser.Id, adminUser.Email));
    }

    private async Task<Guid> SeedAdminRoleAsync(Guid tenantId, CancellationToken cancellationToken)
    {
        var adminRole = new Role
        {
            Id = Guid.NewGuid(),
            TenantId = tenantId,
            Nome = "Administrador",
            Descricao = "Acesso total — role de sistema criada automaticamente.",
            IsSystem = true,
            CreatedAt = DateTime.UtcNow
        };
        await _roles.AddAsync(adminRole, cancellationToken);

        var allPermissions = await _permissions.ListAllAsync(cancellationToken);
        if (allPermissions.Count > 0)
        {
            await _rolePermissions.GrantAllToRoleAsync(
                adminRole.Id,
                allPermissions.Select(p => p.Id),
                grantedBy: null,
                cancellationToken);
        }
        return adminRole.Id;
    }

    private async Task<UsuarioEntity> SeedAdminUserAsync(
        Guid tenantId,
        Guid adminRoleId,
        CriarTenantCommand request,
        CancellationToken cancellationToken)
    {
        var rawToken = ConfirmationTokenHelper.Generate();
        var tokenHash = ConfirmationTokenHelper.HashToken(rawToken);
        var expiresAt = DateTime.UtcNow.Add(ConfirmationTokenLifetime);

        var usuario = new UsuarioEntity
        {
            TenantId = tenantId,
            NomeCompleto = request.AdminNomeCompleto,
            Email = request.AdminEmail,
            PasswordHash = PasswordHelper.Hash(request.AdminSenha),
            Status = StatusAtivo.PendenteConfirmacao,
            EmailConfirmationTokenHash = tokenHash,
            EmailConfirmationExpiresAt = expiresAt
        };
        await _usuarios.AddAsync(usuario, cancellationToken);

        await _userRoles.AssignAsync(new UserRole
        {
            UserId = usuario.Id,
            RoleId = adminRoleId,
            TenantId = tenantId,
            GrantedAt = DateTime.UtcNow
        }, cancellationToken);

        var confirmationUrl = _publicApp.BuildEmailConfirmationUrl(rawToken);
        var body = $@"<p>Olá {System.Net.WebUtility.HtmlEncode(request.AdminNomeCompleto)},</p>
<p>Para ativar sua conta no Atena ERP, clique no link abaixo (válido por 24 horas):</p>
<p><a href=""{confirmationUrl}"">Confirmar e-mail</a></p>
<p>Se você não solicitou este cadastro, ignore esta mensagem.</p>";

        await _emails.EnqueueAsync(new EmailMessage(
            To: request.AdminEmail,
            Subject: "Atena ERP — confirme seu e-mail",
            Body: body,
            IsHtml: true), cancellationToken);

        return usuario;
    }

    private static TenantLimites BuildLimitesFor(Guid tenantId, string plano) => new()
    {
        TenantId = tenantId,
        MaxUsuarios = plano switch
        {
            "FREE" => 3, "BASIC" => 10, "PRO" => 50, "ENTERPRISE" => 999, _ => 3
        },
        MaxNFeMes = plano switch
        {
            "FREE" => 50, "BASIC" => 500, "PRO" => 5000, "ENTERPRISE" => 999_999, _ => 50
        },
        MaxStorageGb = plano switch
        {
            "FREE" => 1, "BASIC" => 10, "PRO" => 100, "ENTERPRISE" => 1000, _ => 1
        }
    };
}
