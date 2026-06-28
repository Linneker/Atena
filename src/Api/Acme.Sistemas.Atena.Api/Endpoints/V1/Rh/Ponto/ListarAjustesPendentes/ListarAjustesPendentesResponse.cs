using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Ponto.ListarAjustesPendentes;

public sealed record ListarAjustesPendentesResponseItem(
    Guid Id, Guid FuncionarioId, Guid? MarcacaoOriginalId,
    TipoAjuste TipoAjuste, DateTime? DataHoraProposta, string Motivo, DateTime SolicitadoEm);

public sealed record ListarAjustesPendentesResponse(
    IReadOnlyList<ListarAjustesPendentesResponseItem> Items, long Total);
