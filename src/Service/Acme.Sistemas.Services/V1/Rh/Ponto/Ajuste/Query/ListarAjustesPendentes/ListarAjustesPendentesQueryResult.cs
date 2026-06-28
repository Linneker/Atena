using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Services.V1.Rh.Ponto.Ajuste.Query.ListarAjustesPendentes;

public sealed record ListarAjustesPendentesQueryItem(
    Guid Id,
    Guid FuncionarioId,
    Guid? MarcacaoOriginalId,
    TipoAjuste TipoAjuste,
    DateTime? DataHoraProposta,
    string Motivo,
    DateTime SolicitadoEm);

public sealed record ListarAjustesPendentesQueryResult(
    IReadOnlyList<ListarAjustesPendentesQueryItem> Items,
    long Total);
