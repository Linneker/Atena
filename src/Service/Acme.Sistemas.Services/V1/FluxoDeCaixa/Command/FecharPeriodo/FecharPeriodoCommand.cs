using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.FluxoDeCaixa.Command.FecharPeriodo;

public sealed record FecharPeriodoCommand(
    int Ano,
    int Mes,
    string? Observacao) : IRequest<ResponseDefault<FecharPeriodoCommandResult>>;

public sealed record FecharPeriodoCommandResult(
    Guid Id,
    int Ano,
    int Mes,
    decimal TotalReceitas,
    decimal TotalDespesas,
    decimal Resultado,
    DateTime FechadoEm);
