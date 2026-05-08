namespace Acme.Sistemas.Domain.Entities.Fiscal.Xml.Servicos;

/// <summary>
/// Códigos de status (cStat) mais comuns retornados pelos serviços SEFAZ.
/// Lista não-exaustiva — códigos não conhecidos são preservados como string.
/// Referência: Manual de Orientação ao Contribuinte (MOC) v7.0 NF-e v4.00.
/// </summary>
public static class SefazResultadoCodigo
{
    // Sucesso de autorização
    public const string Autorizado100 = "100";
    public const string LoteRecebido103 = "103";
    public const string LoteProcessado104 = "104";
    public const string LoteEmProcessamento105 = "105";

    // Status de serviço
    public const string ServicoOperando107 = "107";
    public const string ServicoParalisadoMomento108 = "108";
    public const string ServicoParalisadoProgramada109 = "109";

    // Eventos
    public const string EventoRegistrado135 = "135";
    public const string EventoVinculado136 = "136";

    // Erros operacionais
    public const string DuplicidadeNFe204 = "204";
    public const string AssinaturaInvalida225 = "225";
    public const string CertificadoIrregular279 = "279";
    public const string CertificadoVencido280 = "280";
    public const string CertificadoRevogado281 = "281";

    // Erro local (gerado pelo cliente, não pela SEFAZ)
    public const string ErroLocalValidacao = "999";

    /// <summary>
    /// True se o cStat indica autorização bem-sucedida (NFe ou evento).
    /// </summary>
    public static bool IsAutorizado(string? cStat) =>
        cStat == Autorizado100 || cStat == EventoRegistrado135 || cStat == EventoVinculado136;

    /// <summary>
    /// True se o cStat indica que o serviço SEFAZ está em paralisação (gatilho de contingência).
    /// </summary>
    public static bool IsParalisacao(string? cStat) =>
        cStat == ServicoParalisadoMomento108 || cStat == ServicoParalisadoProgramada109;
}
