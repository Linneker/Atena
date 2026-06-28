using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Admin.Command.SeedTenant;

/// <summary>
/// Provisiona um tenant idempotentemente (chave: CNPJ), criando tenant + admin + empresa demo +
/// plano de contas + centros de custo + cliente/fornecedor/produto demo + config fiscal placeholder.
/// </summary>
public sealed record SeedTenantCommand(string Cnpj, string RazaoSocial, string AdminEmail)
    : IRequest<ResponseDefault<SeedTenantCommandResult>>;
