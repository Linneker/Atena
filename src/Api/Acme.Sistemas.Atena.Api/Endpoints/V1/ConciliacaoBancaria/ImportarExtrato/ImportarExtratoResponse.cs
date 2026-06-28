namespace Acme.Sistemas.Atena.Api.Endpoints.V1.ConciliacaoBancaria.ImportarExtrato;

public sealed record ImportarExtratoResponse(
    Guid ConciliacaoId,
    int TotalLancamentos,
    int TotalConciliados);
