using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Services.V1.Rh.Ponto.Marcacao.Command.BaterPontoMobile;

/// <summary>
/// Batida vinda do app mobile. Validações adicionais frente ao BaterPonto web:
///   - deviceId DEVE estar registrado e ativo para o usuário
///   - timestampLocal não pode diferir do servidor em mais de ± 5min
///   - hashBatida confere (calculado pelo app)
///   - foto OU provaBiometriaLocal (pelo menos um)
/// Foto é persistida em S3/GED via FotoUrl preenchida no handler (upload depois).
/// </summary>
public sealed record BaterPontoMobileCommand(
    TipoMarcacao? Tipo,
    decimal? Latitude,
    decimal? Longitude,
    string DeviceId,
    DateTime TimestampLocal,
    string HashBatida,
    string? ProvaBiometriaLocal,
    byte[]? FotoBytes,
    string? FotoContentType) : IRequest<ResponseDefault<BaterPontoMobileCommandResult>>;
