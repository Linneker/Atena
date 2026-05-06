using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Tenant.Query.ObterTenant;

public sealed class ObterTenantQueryHandler
    : IRequestHandler<ObterTenantQuery, ResponseDefault<ObterTenantQueryResult>>
{
    private readonly ITenantRepository _repository;

    public ObterTenantQueryHandler(ITenantRepository repository)
    {
        _repository = repository;
    }

    public async Task<ResponseDefault<ObterTenantQueryResult>> Handle(
        ObterTenantQuery request,
        CancellationToken cancellationToken)
    {
        var tenant = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (tenant is null)
        {
            return ResponseDefault<ObterTenantQueryResult>.NotFound("Tenant não encontrado.");
        }

        var result = new ObterTenantQueryResult(
            tenant.Id, tenant.RazaoSocial, tenant.Cnpj, tenant.Plano,
            (int)tenant.Status, tenant.LogoUrl, tenant.CorPrimaria,
            tenant.FusoHorario, tenant.CreatedAt);

        return ResponseDefault<ObterTenantQueryResult>.Ok(result);
    }
}
