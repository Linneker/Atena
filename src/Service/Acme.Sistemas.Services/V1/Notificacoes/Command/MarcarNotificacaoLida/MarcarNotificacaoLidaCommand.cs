using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Notificacoes.Command.MarcarNotificacaoLida;

public sealed record MarcarNotificacaoLidaCommand(Guid Id)
    : IRequest<ResponseDefault<MarcarNotificacaoLidaCommandResult>>;
