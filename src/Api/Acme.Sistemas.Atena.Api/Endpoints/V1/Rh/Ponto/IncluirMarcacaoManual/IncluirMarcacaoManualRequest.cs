using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Ponto.IncluirMarcacaoManual;

public sealed record IncluirMarcacaoManualRequest(
    Guid FuncionarioId,
    DateTime DataHora,
    TipoMarcacao Tipo,
    string Motivo);
