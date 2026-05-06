using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Tenant.Query.ListarTenants;

public sealed class ListarTenantsQueryHandler
    : IRequestHandler<ListarTenantsQuery, ResponseDefault<ListarTenantsQueryResult>>
{
    private readonly ITenantRepository _repository;

    public ListarTenantsQueryHandler(ITenantRepository repository)
    {
        _repository = repository;
    }

    public async Task<ResponseDefault<ListarTenantsQueryResult>> Handle(
        ListarTenantsQuery request,
        CancellationToken cancellationToken)
    {
        var tenants = await _repository.ListAsync(request.Skip, request.Take, cancellationToken);

        var items = tenants.Select(t => new ListarTenantsQueryItem(
            t.Id, t.RazaoSocial, t.Cnpj, t.Plano, (int)t.Status, t.CreatedAt)).ToList();

        return ResponseDefault<ListarTenantsQueryResult>.Ok(
            new ListarTenantsQueryResult(items, items.Count));
    }
}
