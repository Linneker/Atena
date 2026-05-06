using Acme.Sistemas.Core.Erros;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Core.Response.Erros;
using Acme.Sistemas.Core.Security;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Autenticacao.Command.ConfirmarEmail;

public sealed class ConfirmarEmailCommandHandler
    : IRequestHandler<ConfirmarEmailCommand, ResponseDefault<ConfirmarEmailCommandResult>>
{
    private readonly IUsuarioRepository _usuarios;

    public ConfirmarEmailCommandHandler(IUsuarioRepository usuarios)
    {
        _usuarios = usuarios;
    }

    public async Task<ResponseDefault<ConfirmarEmailCommandResult>> Handle(
        ConfirmarEmailCommand request,
        CancellationToken cancellationToken)
    {
        var hash = ConfirmationTokenHelper.HashToken(request.Token);
        var usuario = await _usuarios.GetByConfirmationTokenAsync(hash, cancellationToken);
        if (usuario is null)
        {
            return ResponseDefault<ConfirmarEmailCommandResult>.BadRequest(
                Error.Validation(MessageErros.TokenConfirmacaoInvalido));
        }

        if (usuario.IsEmailConfirmed)
        {
            return ResponseDefault<ConfirmarEmailCommandResult>.Conflict(
                "E-mail já foi confirmado anteriormente.");
        }

        if (!usuario.EmailConfirmationExpiresAt.HasValue ||
            usuario.EmailConfirmationExpiresAt.Value < DateTime.UtcNow)
        {
            return ResponseDefault<ConfirmarEmailCommandResult>.BadRequest(
                Error.Validation(MessageErros.TokenConfirmacaoInvalido));
        }

        var now = DateTime.UtcNow;
        await _usuarios.ConfirmEmailAsync(usuario.Id, now, cancellationToken);

        return ResponseDefault<ConfirmarEmailCommandResult>.Ok(
            new ConfirmarEmailCommandResult(usuario.Id, usuario.Email, now));
    }
}
