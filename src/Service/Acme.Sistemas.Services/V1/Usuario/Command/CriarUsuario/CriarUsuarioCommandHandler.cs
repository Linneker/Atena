using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Core.Security;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Usuario.Command.CriarUsuario;

public sealed class CriarUsuarioCommandHandler
    : IRequestHandler<CriarUsuarioCommand, ResponseDefault<CriarUsuarioCommandResult>>
{
    private readonly IUsuarioRepository _usuarios;
    private readonly ITenantContext _tenantContext;

    public CriarUsuarioCommandHandler(IUsuarioRepository usuarios, ITenantContext tenantContext)
    {
        _usuarios = usuarios;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<CriarUsuarioCommandResult>> Handle(
        CriarUsuarioCommand request,
        CancellationToken cancellationToken)
    {
        if (!PasswordHelper.IsStrong(request.Senha))
        {
            return ResponseDefault<CriarUsuarioCommandResult>.BadRequest(
                Core.Response.Erros.Error.Validation(
                    "Senha fraca. Deve conter ao menos 8 caracteres com maiúsculas, minúsculas, dígitos e símbolos."));
        }

        var existing = await _usuarios.GetByEmailAsync(_tenantContext.TenantId, request.Email, cancellationToken);
        if (existing is not null)
        {
            return ResponseDefault<CriarUsuarioCommandResult>.Conflict(
                $"Já existe um usuário com o e-mail {request.Email}.");
        }

        var usuario = new Domain.Entities.Users.Usuario
        {
            TenantId = _tenantContext.TenantId,
            NomeCompleto = request.NomeCompleto,
            Email = request.Email,
            PasswordHash = PasswordHelper.Hash(request.Senha),
            Status = StatusAtivo.Ativo,
            CreatedBy = _tenantContext.UserId
        };

        await _usuarios.AddAsync(usuario, cancellationToken);

        return ResponseDefault<CriarUsuarioCommandResult>.Created(
            new CriarUsuarioCommandResult(usuario.Id, usuario.NomeCompleto, usuario.Email));
    }
}
