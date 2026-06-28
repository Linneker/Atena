using Acme.Sistemas.Services.V1.Usuario.Command.ExcluirUsuario;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Usuarios.ExcluirUsuario;

public static class ExcluirUsuarioMap
{
    public static ExcluirUsuarioCommand ToCommand(this ExcluirUsuarioRequest request)
        => new(request.Id);
}
