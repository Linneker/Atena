using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Ponto.BaterPonto;

public sealed record BaterPontoResponse(
    Guid Id,
    DateTime DataHora,
    TipoMarcacao Tipo,
    string HashIntegridade);
