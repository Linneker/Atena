using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.Ponto.Ajuste.Command.AprovarAjuste;

public sealed record AprovarAjusteCommand(Guid Id, string? Justificativa)
    : IRequest<ResponseDefault<AprovarAjusteCommandResult>>;
