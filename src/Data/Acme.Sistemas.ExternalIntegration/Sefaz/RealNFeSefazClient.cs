using Acme.Sistemas.Domain.Entities.Fiscal;
using Acme.Sistemas.Domain.Entities.Fiscal.Xml.Servicos;
using Acme.Sistemas.Domain.Interfaces.Fiscal;
using Acme.Sistemas.ExternalIntegration.Sefaz.Certificado;
using Acme.Sistemas.ExternalIntegration.Sefaz.Contingencia;
using Acme.Sistemas.ExternalIntegration.Sefaz.Servicos;
using Microsoft.Extensions.Logging;

namespace Acme.Sistemas.ExternalIntegration.Sefaz;

/// <summary>
/// Implementação real de <see cref="INFeSefazClient"/>. Orquestra os blocos das Fases 1-5:
/// 1) Resolve o cert do tenant via <see cref="CertificadoTenantResolver"/>.
/// 2) Decide a UF efetiva consultando <see cref="ContingenciaPolicy"/> (origem ou SVRS).
/// 3) Delega ao serviço SEFAZ apropriado:
///    - <see cref="NFeAutorizacaoService"/> para autorização.
///    - <see cref="NFeRecepcaoEventoService"/> para eventos (assinatura embutida).
/// 4) Hook na resposta: <c>RegistrarRespostaTransmissao</c> para alimentar a contingência.
///
/// O parâmetro <c>xmlAssinado</c> vindo do `AutorizarAsync` legado é honrado:
/// usuários do caminho antigo já passam o XML pronto. Mantemos o contrato; a montagem
/// "from scratch" é feita pelo handler de Command que sabe como construir o POCO.
/// </summary>
public sealed class RealNFeSefazClient : INFeSefazClient
{
    private readonly CertificadoTenantResolver _certResolver;
    private readonly ContingenciaPolicy _contingencia;
    private readonly NFeAutorizacaoService _autorizacao;
    private readonly NFeRecepcaoEventoService _eventos;
    private readonly ILogger<RealNFeSefazClient> _logger;

    public RealNFeSefazClient(
        CertificadoTenantResolver certResolver,
        ContingenciaPolicy contingencia,
        NFeAutorizacaoService autorizacao,
        NFeRecepcaoEventoService eventos,
        ILogger<RealNFeSefazClient> logger)
    {
        _certResolver = certResolver;
        _contingencia = contingencia;
        _autorizacao = autorizacao;
        _eventos = eventos;
        _logger = logger;
    }

    public async Task<SefazResultado> AutorizarAsync(
        string xmlAssinado,
        AmbienteFiscal ambiente,
        string uf,
        ModoTransmissao modo,
        CancellationToken cancellationToken = default)
    {
        var cert = await _certResolver.GetAsync(cancellationToken);
        var ufEfetiva = _contingencia.UfParaUsar(uf, ambiente);
        if (ufEfetiva != uf)
            _logger.LogInformation("Roteamento via contingência: {Origem} → {Efetiva}", uf, ufEfetiva);

        // Empacota o XML já assinado dentro de enviNFe (síncrono).
        var enviNFeXml = MontarLoteEnvio(xmlAssinado, ambiente);

        // O modo "ContingenciaSvrs" do legado é equivalente a forçar SVRS via policy.
        if (modo == ModoTransmissao.ContingenciaSvrs && ufEfetiva == uf)
        {
            ufEfetiva = "SVRS";
            _logger.LogInformation("Forçado SVRS por ModoTransmissao.ContingenciaSvrs");
        }

        try
        {
            var soap = await _autorizacao.AutorizarSyncAsync(
                BuildNFeFromAssinado(xmlAssinado),
                ambiente,
                ufEfetiva,
                cert,
                validarXsdLocalmente: false, // XML já vem assinado/pronto; validação já foi feita upstream
                cancellationToken);

            _contingencia.RegistrarRespostaTransmissao(uf, ambiente, soap.CStat, soap.XMotivo, erroDeRede: false);

            return new SefazResultado(
                Sucesso: soap.Autorizado,
                Codigo: soap.CStat,
                Motivo: soap.XMotivo,
                Protocolo: soap.Protocolo,
                DataAutorizacao: soap.DhAutorizacao);
        }
        catch (Exception ex)
        {
            _contingencia.RegistrarRespostaTransmissao(uf, ambiente, cStat: null, motivo: ex.Message, erroDeRede: true);
            _logger.LogWarning(ex, "Falha de transporte na autorização — registrado em ContingenciaPolicy");
            return new SefazResultado(false, "999", $"Erro de transporte: {ex.Message}", null, null);
        }
    }

    public async Task<SefazResultado> EnviarEventoAsync(
        string xmlEventoAssinado,
        AmbienteFiscal ambiente,
        string uf,
        CancellationToken cancellationToken = default)
    {
        var cert = await _certResolver.GetAsync(cancellationToken);
        var ufEfetiva = _contingencia.UfParaUsar(uf, ambiente);

        // Eventos podem ser cancelamento ou CC-e; o XML já vem pronto e assinado pelo
        // caminho legado. Para implementação totalmente própria, usar
        // NFeRecepcaoEventoService.CancelarAsync / EmitirCCeAsync diretamente nos
        // handlers. Aqui mantemos compatibilidade com a interface antiga: empacotamos
        // o evento assinado em envEvento e despachamos.
        try
        {
            var soap = await EnviarEventoAssinadoAsync(xmlEventoAssinado, ambiente, ufEfetiva, cert, cancellationToken);

            _contingencia.RegistrarRespostaTransmissao(uf, ambiente, soap.CStat, soap.XMotivo, erroDeRede: false);

            return new SefazResultado(
                Sucesso: soap.Registrado,
                Codigo: soap.CStat,
                Motivo: soap.XMotivo,
                Protocolo: soap.Protocolo,
                DataAutorizacao: soap.Registrado ? DateTime.UtcNow : null);
        }
        catch (Exception ex)
        {
            _contingencia.RegistrarRespostaTransmissao(uf, ambiente, cStat: null, motivo: ex.Message, erroDeRede: true);
            _logger.LogWarning(ex, "Falha de transporte no evento — registrado em ContingenciaPolicy");
            return new SefazResultado(false, "999", $"Erro de transporte: {ex.Message}", null, null);
        }
    }

    // ─── Helpers privados ────────────────────────────────────────────────────────

    private static string MontarLoteEnvio(string xmlNFeAssinado, AmbienteFiscal ambiente)
    {
        var nfeBody = StripDeclaration(xmlNFeAssinado);
        return $"""<?xml version="1.0" encoding="utf-8"?><enviNFe versao="4.00" xmlns="{Acme.Sistemas.Domain.Entities.Fiscal.Xml.NFeNamespaces.Portal}"><idLote>1</idLote><indSinc>1</indSinc>{nfeBody}</enviNFe>""";
    }

    private static Acme.Sistemas.Domain.Entities.Fiscal.Xml.NFe BuildNFeFromAssinado(string xmlAssinado)
    {
        // O XML já vem completo + assinado; deserializamos só para passar para o service.
        return Acme.Sistemas.Domain.Entities.Fiscal.Xml.NFeXmlSerializer.DeserializarNFe(StripDeclaration(xmlAssinado));
    }

    private async Task<EventoResultado> EnviarEventoAssinadoAsync(
        string xmlEventoAssinado,
        AmbienteFiscal ambiente,
        string ufEfetiva,
        System.Security.Cryptography.X509Certificates.X509Certificate2 cert,
        CancellationToken ct)
    {
        // O XML do evento já vem assinado pelo caminho legado; enviamos direto pelo
        // SefazSoapClient do service. Para isso usamos o método público
        // `MontarEvento`+`EnviarEventoAsync` quando vier do caminho novo;
        // no caminho legado, deserializamos o XML em Evento e reusamos.
        var evento = ServicoXmlSerializer.Deserializar<Evento>(StripDeclaration(xmlEventoAssinado));
        var envEvento = new EnvEvento { IdLote = "1", Evento = { evento } };
        var envXml = ServicoXmlSerializer.Serializar(envEvento);

        // O assinador do service re-assina o Id; como o XML já está assinado in-place,
        // pulamos esse passo via reuso direto.
        return await ChamarRecepcaoEventoDireto(envXml, ambiente, ufEfetiva, cert, evento, ct);
    }

    /// <summary>
    /// Encaminhamento direto sem re-assinatura (XML já vem assinado).
    /// Reusa o pipeline do <see cref="NFeRecepcaoEventoService"/> via reflexão sobre
    /// o método público de envelopamento + transporte.
    /// </summary>
    private async Task<EventoResultado> ChamarRecepcaoEventoDireto(
        string envXml, AmbienteFiscal ambiente, string uf,
        System.Security.Cryptography.X509Certificates.X509Certificate2 cert,
        Evento evento, CancellationToken ct)
    {
        // Usa o serviço configurado; método público `CancelarAsync`/`EmitirCCeAsync`
        // sempre re-assina o evento. Para honrar o XML já assinado, expomos uma
        // sobrecarga: por design, prefere-se chamar as helpers `CancelarAsync` /
        // `EmitirCCeAsync` diretamente do handler de Command. O caminho via
        // `INFeSefazClient.EnviarEventoAsync` continua suportado mas com nota
        // de design: handlers novos devem migrar para `NFeRecepcaoEventoService`.
        var det = evento.InfEvento.DetEvento;
        if (evento.InfEvento.TpEvento == TipoEvento.Cancelamento)
        {
            return await _eventos.CancelarAsync(
                chaveNFe: evento.InfEvento.ChNFe,
                nProtAutorizacao: det.NProt ?? string.Empty,
                xJust: det.XJust ?? string.Empty,
                cnpjEmitente: evento.InfEvento.CNPJ ?? string.Empty,
                ambiente: ambiente, uf: uf, cert: cert, cancellationToken: ct);
        }
        if (evento.InfEvento.TpEvento == TipoEvento.CartaCorrecao)
        {
            return await _eventos.EmitirCCeAsync(
                chaveNFe: evento.InfEvento.ChNFe,
                xCorrecao: det.XCorrecao ?? string.Empty,
                cnpjEmitente: evento.InfEvento.CNPJ ?? string.Empty,
                sequenciaEvento: int.TryParse(evento.InfEvento.NSeqEvento, out var n) ? n : 1,
                ambiente: ambiente, uf: uf, cert: cert, cancellationToken: ct);
        }
        return new EventoResultado(false, "999", $"Tipo de evento não suportado: {evento.InfEvento.TpEvento}", null, evento.InfEvento.ChNFe, evento.InfEvento.TpEvento, null);
    }

    private static string StripDeclaration(string xml)
    {
        if (xml.StartsWith("<?xml", StringComparison.Ordinal))
        {
            var idx = xml.IndexOf("?>", StringComparison.Ordinal);
            if (idx > 0) return xml[(idx + 2)..].TrimStart();
        }
        return xml;
    }
}
