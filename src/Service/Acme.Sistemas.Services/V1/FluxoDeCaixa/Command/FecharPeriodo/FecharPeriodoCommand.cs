using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.FluxoDeCaixa.Command.FecharPeriodo;

public sealed record FecharPeriodoCommand(
    int Ano,
    int Mes,
    string? Observacao) : IRequest<ResponseDefault<FecharPeriodoCommandResult>>;

