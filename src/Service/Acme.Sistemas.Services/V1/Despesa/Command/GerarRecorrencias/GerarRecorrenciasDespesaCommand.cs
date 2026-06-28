using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Despesa.Command.GerarRecorrencias;

/// <summary>
/// Para cada Despesa com DespesaFixa=true do tenant, gera entries nos próximos
/// <paramref name="Meses"/> meses caso ainda não existam (critério: mesmo Nome no ano-mês).
/// </summary>
public sealed record GerarRecorrenciasDespesaCommand(int Meses = 3)
    : IRequest<ResponseDefault<GerarRecorrenciasDespesaCommandResult>>;
