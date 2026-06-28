using Acme.Sistemas.Services.V1.Usuario.Query.ObterUsuario;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Usuarios.ObterUsuario;

public static class ObterUsuarioMap
{
    public static ObterUsuarioQuery ToQuery(this ObterUsuarioRequest request)
        => new(request.Id);

    public static ObterUsuarioResponse ToResponse(this ObterUsuarioQueryResult result)
        => new(
            result.Id,
            result.NomeCompleto,
            result.Email,
            result.Status,
            result.FailedLoginAttempts,
            result.LockedUntil,
            result.LastLoginAt,
            result.CreatedAt,
            result.UpdatedAt);
}
