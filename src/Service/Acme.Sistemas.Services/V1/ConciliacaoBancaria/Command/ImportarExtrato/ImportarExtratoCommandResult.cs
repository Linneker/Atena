using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.ConciliacaoBancaria.Command.ImportarExtrato;

public sealed record ImportarExtratoCommandResult(
    Guid ConciliacaoId,
    int TotalLancamentos,
    int TotalConciliados);
