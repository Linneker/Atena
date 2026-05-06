using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Usuario.Command.AlterarUsuario;

public sealed class AlterarUsuarioCommandHandler
    : IRequestHandler<AlterarUsuarioCommand, ResponseDefault<AlterarUsuarioCommandResult>>
{
    private readonly IUsuarioRepository _usuarios;
    private readonly ITenantContext _tenantContext;

    public AlterarUsuarioCommandHandler(IUsuarioRepository usuarios, ITenantContext tenantContext)
    {
        _usuarios = usuarios;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<AlterarUsuarioCommandResult>> Handle(
        AlterarUsuarioCommand request,
        CancellationToken cancellationToken)
    {
        var usuario = await _usuarios.GetByIdAsync(request.Id, cancellationToken);
        if (usuario is null)
        {
            return ResponseDefault<AlterarUsuarioCommandResult>.NotFound("Usuário não encontrado.");
        }

        if (!string.Equals(usuario.Email, request.Email, StringComparison.OrdinalIgnoreCase))
        {
            var existing = await _usuarios.GetByEmailAsync(_tenantContext.TenantId, request.Email, cancellationToken);
            if (existing is not null && existing.Id != usuario.Id)
            {
                return ResponseDefault<AlterarUsuarioCommandResult>.Conflict(
                    $"Já existe um usuário com o e-mail {request.Email}.");
            }
        }

        usuario.NomeCompleto = request.NomeCompleto;
        usuario.Email = request.Email;
        usuario.Status = request.Status;
        usuario.UpdatedBy = _tenantContext.UserId;

        await _usuarios.UpdateAsync(usuario, cancellationToken);

        return ResponseDefault<AlterarUsuarioCommandResult>.Ok(new AlterarUsuarioCommandResult(usuario.Id));
    }
}
