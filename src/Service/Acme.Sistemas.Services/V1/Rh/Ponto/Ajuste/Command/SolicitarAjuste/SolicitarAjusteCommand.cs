using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Services.V1.Rh.Ponto.Ajuste.Command.SolicitarAjuste;

public sealed record SolicitarAjusteCommand(
    Guid? MarcacaoOriginalId,
    TipoAjuste TipoAjuste,
    DateTime? DataHoraProposta,
    TipoMarcacao? TipoMarcacaoProposta,
    string Motivo,
    string? AnexoUrl) : IRequest<ResponseDefault<SolicitarAjusteCommandResult>>;
