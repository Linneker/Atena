namespace Acme.Sistemas.Atena.Mobile.Shared.Dtos;

public enum TipoMarcacaoDto { Entrada, SaidaAlmoco, VoltaAlmoco, Saida, Pausa, RetornoPausa }
public enum OrigemMarcacaoDto { Web, MobileApp, Kiosk, Manual, Importacao }
public enum StatusMarcacaoDto { Valida, AjusteSolicitado, Ajustada, Invalida }
public enum TipoAjusteDto { AlteracaoHora, Inclusao, Exclusao, Justificativa }
public enum StatusAjusteDto { Pendente, Aprovado, Rejeitado, Cancelado }

/// <summary>Form fields enviados em multipart junto da foto JPEG.</summary>
public sealed record BaterPontoMobileForm(
    TipoMarcacaoDto? Tipo,
    decimal? Latitude,
    decimal? Longitude,
    string DeviceId,
    DateTime TimestampLocal,
    string HashBatida,
    string? ProvaBiometriaLocal);

public sealed record BaterPontoResponse(
    string Id,
    DateTime DataHora,
    TipoMarcacaoDto Tipo,
    string HashIntegridade,
    string? FotoUrl);

public sealed record MarcacaoDto(
    string Id,
    DateTime DataHora,
    TipoMarcacaoDto Tipo,
    OrigemMarcacaoDto Origem,
    StatusMarcacaoDto Status,
    string HashIntegridade,
    string? FotoUrl);

public sealed record SolicitarAjusteRequest(
    string? MarcacaoOriginalId,
    TipoAjusteDto TipoAjuste,
    DateTime? DataHoraProposta,
    TipoMarcacaoDto? TipoMarcacaoProposta,
    string Motivo,
    string? AnexoUrl);

public sealed record AjusteDto(
    string Id,
    string FuncionarioId,
    string? MarcacaoOriginalId,
    TipoAjusteDto TipoAjuste,
    DateTime? DataHoraProposta,
    string Motivo,
    StatusAjusteDto Status,
    DateTime SolicitadoEm);

public sealed record EspelhoBatidaDto(string Id, string Hora, string Tipo, string Origem);
public sealed record EspelhoDiaDto(
    DateOnly Data,
    string DiaSemana,
    bool EhFeriado,
    bool EhDiaUtil,
    string? JanelaEsperadaEntrada,
    string? JanelaEsperadaSaida,
    IReadOnlyList<EspelhoBatidaDto> Batidas,
    int TrabalhadoMinutos,
    int EsperadoMinutos,
    int SaldoMinutos,
    int AtrasoMinutos,
    IReadOnlyList<string> Anomalias);

public sealed record EspelhoMensalDto(
    string FuncionarioId,
    string FuncionarioNome,
    string Competencia,
    IReadOnlyList<EspelhoDiaDto> Dias,
    int TrabalhadoMinutos,
    int EsperadoMinutos,
    int SaldoMesMinutos,
    string HashEspelho);
