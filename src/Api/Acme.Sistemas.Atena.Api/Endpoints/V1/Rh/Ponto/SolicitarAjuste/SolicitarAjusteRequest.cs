using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Ponto.SolicitarAjuste;

public sealed record SolicitarAjusteRequest(
    Guid? MarcacaoOriginalId,
    TipoAjuste TipoAjuste,
    DateTime? DataHoraProposta,
    TipoMarcacao? TipoMarcacaoProposta,
    string Motivo,
    string? AnexoUrl);
