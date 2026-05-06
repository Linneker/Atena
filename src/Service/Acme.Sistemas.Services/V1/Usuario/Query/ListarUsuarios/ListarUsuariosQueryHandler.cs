using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Usuario.Query.ListarUsuarios;

public sealed class ListarUsuariosQueryHandler
    : IRequestHandler<ListarUsuariosQuery, ResponseDefault<ListarUsuariosQueryResult>>
{
    private readonly IUsuarioRepository _usuarios;

    public ListarUsuariosQueryHandler(IUsuarioRepository usuarios)
    {
        _usuarios = usuarios;
    }

    public async Task<ResponseDefault<ListarUsuariosQueryResult>> Handle(
        ListarUsuariosQuery request,
        CancellationToken cancellationToken)
    {
        var usuarios = await _usuarios.ListAsync(request.Skip, request.Take, cancellationToken);

        var items = usuarios.Select(u => new ListarUsuariosQueryItem(
            u.Id, u.NomeCompleto, u.Email, u.Status, u.LastLoginAt, u.CreatedAt)).ToList();

        return ResponseDefault<ListarUsuariosQueryResult>.Ok(
            new ListarUsuariosQueryResult(items, items.Count));
    }
}
