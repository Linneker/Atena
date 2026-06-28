using Acme.Sistemas.Services.V1.Usuario.Command.AlterarUsuario;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Usuarios.AlterarUsuario;

public static class AlterarUsuarioMap
{
    public static AlterarUsuarioCommand ToCommand(this AlterarUsuarioRequest request, Guid id)
        => new(id, request.NomeCompleto, request.Email, request.Status);

    public static AlterarUsuarioResponse ToResponse(this AlterarUsuarioCommandResult result)
        => new(result.Id);
}
