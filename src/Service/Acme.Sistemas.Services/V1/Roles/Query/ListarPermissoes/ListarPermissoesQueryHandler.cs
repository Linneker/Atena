using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Roles.Query.ListarPermissoes;

public sealed class ListarPermissoesQueryHandler
    : IRequestHandler<ListarPermissoesQuery, ResponseDefault<ListarPermissoesQueryResult>>
{
    private readonly IPermissionRepository _permissions;

    public ListarPermissoesQueryHandler(IPermissionRepository permissions) { _permissions = permissions; }

    public async Task<ResponseDefault<ListarPermissoesQueryResult>> Handle(
        ListarPermissoesQuery request,
        CancellationToken cancellationToken)
    {
        var perms = await _permissions.ListAllAsync(cancellationToken);
        var items = perms.Select(p => new ListarPermissoesQueryItem(p.Codigo, p.Recurso, p.Acao, p.Descricao)).ToList();
        return ResponseDefault<ListarPermissoesQueryResult>.Ok(new ListarPermissoesQueryResult(items));
    }
}
