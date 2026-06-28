using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Ponto.Ajuste.Command.RejeitarAjuste;

public sealed record RejeitarAjusteCommand(Guid Id, string Justificativa)
    : IRequest<ResponseDefault<RejeitarAjusteCommandResult>>;
