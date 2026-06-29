using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Services.V1.Rh.Ponto.Marcacao.Command.BaterPonto;

public sealed record BaterPontoCommandResult(
    Guid Id,
    DateTime DataHora,
    TipoMarcacao Tipo,
    string HashIntegridade,
    /// <summary>NSR atribuído quando a empresa usa REP oficial (Portaria 671). NULL caso contrário.</summary>
    long? Nsr = null,
    /// <summary>Id do <c>ComprovantePonto</c> assinado emitido (671). NULL caso contrário.</summary>
    Guid? ComprovanteId = null,
    /// <summary>URL para download do PDF do comprovante (2ª via). NULL caso contrário.</summary>
    string? PdfUrl = null);
