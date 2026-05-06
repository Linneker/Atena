using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Entities.Permissions;
using Acme.Sistemas.Domain.Entities.Tenants;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Tenant.Command.CriarTenant;

public sealed class CriarTenantCommandHandler
    : IRequestHandler<CriarTenantCommand, ResponseDefault<CriarTenantCommandResult>>
{
    private readonly ITenantRepository _tenants;
    private readonly IRoleRepository _roles;
    private readonly IPermissionRepository _permissions;
    private readonly IRolePermissionRepository _rolePermissions;

    public CriarTenantCommandHandler(
        ITenantRepository tenants,
        IRoleRepository roles,
        IPermissionRepository permissions,
        IRolePermissionRepository rolePermissions)
    {
        _tenants = tenants;
        _roles = roles;
        _permissions = permissions;
        _rolePermissions = rolePermissions;
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

        await SeedAdminRoleAsync(tenant.Id, cancellationToken);

        return ResponseDefault<CriarTenantCommandResult>.Created(
            new CriarTenantCommandResult(tenant.Id, tenant.RazaoSocial, tenant.Cnpj, tenant.Plano));
    }

    private async Task SeedAdminRoleAsync(Guid tenantId, CancellationToken cancellationToken)
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
