using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Lotacao.Command.RemoverLotacao;

public sealed record RemoverLotacaoCommand(Guid Id)
    : IRequest<ResponseDefault<RemoverLotacaoCommandResult>>;
