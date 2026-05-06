using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Usuario.Query.ObterUsuario;

public sealed class ObterUsuarioQueryHandler
    : IRequestHandler<ObterUsuarioQuery, ResponseDefault<ObterUsuarioQueryResult>>
{
    private readonly IUsuarioRepository _usuarios;

    public ObterUsuarioQueryHandler(IUsuarioRepository usuarios)
    {
        _usuarios = usuarios;
    }

    public async Task<ResponseDefault<ObterUsuarioQueryResult>> Handle(
        ObterUsuarioQuery request,
        CancellationToken cancellationToken)
    {
        var u = await _usuarios.GetByIdAsync(request.Id, cancellationToken);
        if (u is null)
        {
            return ResponseDefault<ObterUsuarioQueryResult>.NotFound("Usuário não encontrado.");
        }

        return ResponseDefault<ObterUsuarioQueryResult>.Ok(new ObterUsuarioQueryResult(
            u.Id, u.NomeCompleto, u.Email, u.Status, u.FailedLoginAttempts,
            u.LockedUntil, u.LastLoginAt, u.CreatedAt, u.UpdatedAt));
    }
}
