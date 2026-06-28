using Acme.Sistemas.Services.V1.Usuario.Command.CriarUsuario;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Usuarios.CriarUsuario;

public static class CriarUsuarioMap
{
    public static CriarUsuarioCommand ToCommand(this CriarUsuarioRequest request)
        => new(request.NomeCompleto, request.Email, request.Senha);

    public static CriarUsuarioResponse ToResponse(this CriarUsuarioCommandResult result)
        => new(result.Id, result.NomeCompleto, result.Email);
}
