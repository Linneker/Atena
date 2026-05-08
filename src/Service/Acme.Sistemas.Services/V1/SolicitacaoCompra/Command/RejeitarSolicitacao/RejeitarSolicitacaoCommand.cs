using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.SolicitacaoCompra.Command.RejeitarSolicitacao;

public sealed record RejeitarSolicitacaoCommand(Guid Id, string Motivo)
    : IRequest<ResponseDefault<RejeitarSolicitacaoCommandResult>>;

