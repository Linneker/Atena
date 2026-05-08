using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Autenticacao.Command.ConfirmarEmail;

public sealed record ConfirmarEmailCommandResult(Guid UserId, string Email, DateTime ConfirmadoEm);
