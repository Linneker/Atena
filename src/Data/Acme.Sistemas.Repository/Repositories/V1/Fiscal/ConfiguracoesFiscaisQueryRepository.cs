using System.Data;
using Acme.Sistemas.Domain.Entities.Fiscal;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Repository.Configuration;
using Acme.Sistemas.Repository.Helper;

namespace Acme.Sistemas.Repository.Repositories.V1.Fiscal;

public sealed class ConfiguracoesFiscaisQueryRepository : IConfiguracoesFiscaisQueryRepository
{
    private readonly IDataConfiguration _db;

    public ConfiguracoesFiscaisQueryRepository(IDataConfiguration db)
    {
        _db = db;
    }

    public Task<IReadOnlyList<ConfiguracaoFiscal>> ListarComCertificadoVencendoAsync(
        DateTime limiteVencimento, CancellationToken cancellationToken = default)
        => _db.QueryAsync(@"
            SELECT id, tenant_id, ambiente, modo, uf, cnpj_emitente, razao_social_emitente,
                   inscricao_estadual, serie_nfe, proximo_numero,
                   certificado_subject, certificado_valido_ate
            FROM configuracao_fiscal
            WHERE deleted_at IS NULL
              AND certificado_valido_ate IS NOT NULL
              AND certificado_valido_ate <= @limite",
            r => new ConfiguracaoFiscal
            {
                Id = r.GetValueOrDefault<Guid>("id"),
                TenantId = r.GetValueOrDefault<Guid>("tenant_id"),
                Ambiente = (AmbienteFiscal)r.GetValueOrDefault<int>("ambiente"),
                Modo = (ModoTransmissao)r.GetValueOrDefault<int>("modo"),
                Uf = r.GetValueOrDefault<string>("uf") ?? "SP",
                CnpjEmitente = r.GetValueOrDefault<string>("cnpj_emitente") ?? string.Empty,
                RazaoSocialEmitente = r.GetValueOrDefault<string>("razao_social_emitente"),
                InscricaoEstadual = r.GetValueOrDefault<string>("inscricao_estadual"),
                SerieNFe = r.GetValueOrDefault<int>("serie_nfe"),
                ProximoNumero = r.GetValueOrDefault<int>("proximo_numero"),
                CertificadoSubject = r.GetValueOrDefault<string>("certificado_subject"),
                CertificadoValidoAte = r.GetValueOrDefault<DateTime?>("certificado_valido_ate")
            },
            new Dictionary<string, object?> { ["@limite"] = limiteVencimento },
            cancellationToken);
}
