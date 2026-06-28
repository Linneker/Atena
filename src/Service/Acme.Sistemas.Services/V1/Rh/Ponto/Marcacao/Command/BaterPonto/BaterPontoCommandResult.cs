using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Services.V1.Rh.Ponto.Marcacao.Command.BaterPonto;

public sealed record BaterPontoCommandResult(
    Guid Id,
    DateTime DataHora,
    TipoMarcacao Tipo,
    string HashIntegridade);
