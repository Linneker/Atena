using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Roles.Query.ListarRoles;

public sealed class ListarRolesQueryHandler
    : IRequestHandler<ListarRolesQuery, ResponseDefault<ListarRolesQueryResult>>
{
    private readonly IRoleRepository _roles;

    public ListarRolesQueryHandler(IRoleRepository roles) { _roles = roles; }

    public async Task<ResponseDefault<ListarRolesQueryResult>> Handle(
        ListarRolesQuery request,
        CancellationToken cancellationToken)
    {
        var roles = await _roles.ListAsync(request.Skip, request.Take, cancellationToken);
        var items = roles.Select(r => new ListarRolesQueryItem(r.Id, r.Nome, r.Descricao, r.IsSystem)).ToList();
        return ResponseDefault<ListarRolesQueryResult>.Ok(new ListarRolesQueryResult(items, items.Count));
    }
}
