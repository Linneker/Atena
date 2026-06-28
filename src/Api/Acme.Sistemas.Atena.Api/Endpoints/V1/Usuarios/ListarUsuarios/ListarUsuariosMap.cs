using Acme.Sistemas.Services.V1.Usuario.Query.ListarUsuarios;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Usuarios.ListarUsuarios;

public static class ListarUsuariosMap
{
    public static ListarUsuariosQuery ToQuery(this ListarUsuariosRequest request)
        => new(request.Skip, request.Take);

    public static ListarUsuariosResponse ToResponse(this ListarUsuariosQueryResult result)
        => new(
            result.Items.Select(i => new ListarUsuariosResponseItem(i.Id, i.NomeCompleto, i.Email, i.Status, i.LastLoginAt, i.CreatedAt)).ToArray(),
            result.Total);
}
