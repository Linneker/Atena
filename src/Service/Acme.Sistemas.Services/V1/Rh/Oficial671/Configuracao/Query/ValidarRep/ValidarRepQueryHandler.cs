using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;
using Acme.Sistemas.ExternalIntegration.Sefaz.Certificado;

namespace Acme.Sistemas.Services.V1.Rh.Oficial671.Configuracao.Query.ValidarRep;

/// <summary>
/// Auto-diagnóstico do REP da empresa: checa se a configuração existe, se o certificado
/// é carregável (e portanto está dentro da validade), e se o CNPJ da config bate com o
/// subject do cert. Retorna lista de checagens (todas devem estar Ok pra empresa ativar
/// <c>usa_rep_oficial=true</c>).
/// </summary>
public sealed class ValidarRepQueryHandler
    : IRequestHandler<ValidarRepQuery, ResponseDefault<ValidarRepQueryResult>>
{
    private readonly IConfiguracaoRepRepository _repo;
    private readonly CertificadoTenantResolver _cert;

    public ValidarRepQueryHandler(IConfiguracaoRepRepository repo, CertificadoTenantResolver cert)
    {
        _repo = repo;
        _cert = cert;
    }

    public async Task<ResponseDefault<ValidarRepQueryResult>> Handle(
        ValidarRepQuery q, CancellationToken cancellationToken)
    {
        var checagens = new List<ValidacaoRepItem>();

        var cfg = await _repo.GetByEmpresaAsync(q.EmpresaId, cancellationToken);
        if (cfg is null)
        {
            checagens.Add(new ValidacaoRepItem("ConfiguracaoRep", false, "Não cadastrada."));
            return ResponseDefault<ValidarRepQueryResult>.Ok(new ValidarRepQueryResult(false, checagens));
        }
        checagens.Add(new ValidacaoRepItem("ConfiguracaoRep", true, null));

        // Tenta carregar cert. Falha = mensagem do CertificadoTenantResolver.
        try
        {
            var c = await _cert.GetAsync(cancellationToken);
            checagens.Add(new ValidacaoRepItem("Certificado", true,
                $"Válido até {c.NotAfter:dd/MM/yyyy} (subject={c.Subject})"));

            // Subject do cert ICP-Brasil tipicamente contém CN=NOME:CNPJ.
            var subj = c.Subject ?? string.Empty;
            var cnpjCfg = cfg.CnpjCei.Trim();
            var bate = !string.IsNullOrEmpty(cnpjCfg) && subj.Contains(cnpjCfg);
            checagens.Add(new ValidacaoRepItem("CnpjVsCertificado", bate,
                bate ? null : $"CNPJ '{cnpjCfg}' não encontrado no subject do cert ('{subj}')."));
        }
        catch (Exception ex)
        {
            checagens.Add(new ValidacaoRepItem("Certificado", false, ex.Message));
            checagens.Add(new ValidacaoRepItem("CnpjVsCertificado", false, "Pulou: cert indisponível."));
        }

        var apto = checagens.TrueForAll(c => c.Ok);
        return ResponseDefault<ValidarRepQueryResult>.Ok(new ValidarRepQueryResult(apto, checagens));
    }
}
