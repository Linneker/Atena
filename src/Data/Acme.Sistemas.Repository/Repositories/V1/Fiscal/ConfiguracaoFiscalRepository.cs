using System.Data;
using Acme.Sistemas.Domain.Entities.Fiscal;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Repository.Configuration;
using Acme.Sistemas.Repository.Helper;

namespace Acme.Sistemas.Repository.Repositories.V1.Fiscal;

public sealed class ConfiguracaoFiscalRepository : IConfiguracaoFiscalRepository
{
    private readonly IDataConfiguration _db;
    private readonly ITenantContext _tenantContext;

    private const string Cols = @"id, tenant_id, ambiente, modo, uf, cnpj_emitente, razao_social_emitente,
        inscricao_estadual, serie_nfe, proximo_numero,
        certificado_pfx_criptografado, certificado_nonce_base64, certificado_subject, certificado_valido_ate,
        certificado_senha_criptografada, certificado_senha_nonce_base64,
        created_at, created_by, updated_at, updated_by, deleted_at, deleted_by";

    public ConfiguracaoFiscalRepository(IDataConfiguration db, ITenantContext tenantContext)
    {
        _db = db;
        _tenantContext = tenantContext;
    }

    public Task<ConfiguracaoFiscal?> GetAsync(CancellationToken cancellationToken = default)
        => _db.QueryFirstOrDefaultAsync(
            $"SELECT {Cols} FROM configuracao_fiscal WHERE tenant_id = @tenantId AND deleted_at IS NULL LIMIT 1",
            Map,
            new Dictionary<string, object?> { ["@tenantId"] = _tenantContext.TenantId },
            cancellationToken);

    public async Task UpsertAsync(ConfiguracaoFiscal c, CancellationToken cancellationToken = default)
    {
        var existing = await GetAsync(cancellationToken);
        if (existing is null)
        {
            await _db.ExecuteAsync(@"
                INSERT INTO configuracao_fiscal
                (id, tenant_id, ambiente, modo, uf, cnpj_emitente, razao_social_emitente,
                 inscricao_estadual, serie_nfe, proximo_numero,
                 certificado_pfx_criptografado, certificado_nonce_base64, certificado_subject, certificado_valido_ate,
                 certificado_senha_criptografada, certificado_senha_nonce_base64,
                 created_at, created_by)
                VALUES
                (@id, @tenant_id, @amb, @modo, @uf, @cnpj, @rs, @ie, @serie, @prox,
                 @pfx, @nonce, @subj, @valido,
                 @sCrip, @sNonce, @created_at, @created_by)",
                BuildParams(c, isInsert: true), cancellationToken);
        }
        else
        {
            c.Id = existing.Id;
            await _db.ExecuteAsync(@"
                UPDATE configuracao_fiscal SET
                    ambiente = @amb, modo = @modo, uf = @uf,
                    cnpj_emitente = @cnpj, razao_social_emitente = @rs,
                    inscricao_estadual = @ie, serie_nfe = @serie, proximo_numero = @prox,
                    certificado_pfx_criptografado = COALESCE(@pfx, certificado_pfx_criptografado),
                    certificado_nonce_base64 = COALESCE(@nonce, certificado_nonce_base64),
                    certificado_subject = COALESCE(@subj, certificado_subject),
                    certificado_valido_ate = COALESCE(@valido, certificado_valido_ate),
                    certificado_senha_criptografada = COALESCE(@sCrip, certificado_senha_criptografada),
                    certificado_senha_nonce_base64 = COALESCE(@sNonce, certificado_senha_nonce_base64),
                    updated_at = @updated_at, updated_by = @updated_by
                WHERE id = @id AND tenant_id = @tenantId AND deleted_at IS NULL",
                BuildParams(c, isInsert: false), cancellationToken);
        }
    }

    public async Task<int> ReservarProximoNumeroAsync(int serie, CancellationToken cancellationToken = default)
    {
        var c = await GetAsync(cancellationToken);
        if (c is null) throw new InvalidOperationException("Configuração fiscal não encontrada para o tenant.");
        var numero = c.ProximoNumero;
        await _db.ExecuteAsync(@"
            UPDATE configuracao_fiscal SET proximo_numero = proximo_numero + 1, updated_at = @now
            WHERE tenant_id = @tenantId AND deleted_at IS NULL",
            new Dictionary<string, object?>
            {
                ["@tenantId"] = _tenantContext.TenantId,
                ["@now"] = DateTime.UtcNow
            }, cancellationToken);
        return numero;
    }

    private Dictionary<string, object?> BuildParams(ConfiguracaoFiscal c, bool isInsert)
    {
        var p = new Dictionary<string, object?>
        {
            ["@id"] = c.Id == Guid.Empty ? Guid.NewGuid() : c.Id,
            ["@amb"] = (int)c.Ambiente,
            ["@modo"] = (int)c.Modo,
            ["@uf"] = c.Uf,
            ["@cnpj"] = c.CnpjEmitente,
            ["@rs"] = c.RazaoSocialEmitente,
            ["@ie"] = c.InscricaoEstadual,
            ["@serie"] = c.SerieNFe,
            ["@prox"] = c.ProximoNumero,
            ["@pfx"] = c.CertificadoPfxCriptografado,
            ["@nonce"] = c.CertificadoNonceBase64,
            ["@subj"] = c.CertificadoSubject,
            ["@valido"] = c.CertificadoValidoAte,
            ["@sCrip"] = c.CertificadoSenhaCriptografada,
            ["@sNonce"] = c.CertificadoSenhaNonceBase64
        };
        c.Id = (Guid)p["@id"]!;
        if (isInsert)
        {
            p["@tenant_id"] = _tenantContext.TenantId;
            p["@created_at"] = c.CreatedAt;
            p["@created_by"] = c.CreatedBy;
        }
        else
        {
            p["@tenantId"] = _tenantContext.TenantId;
            p["@updated_at"] = DateTime.UtcNow;
            p["@updated_by"] = c.UpdatedBy;
        }
        return p;
    }

    private static ConfiguracaoFiscal Map(IDataRecord r) => new()
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
        CertificadoPfxCriptografado = r.GetValueOrDefault<byte[]?>("certificado_pfx_criptografado"),
        CertificadoNonceBase64 = r.GetValueOrDefault<string>("certificado_nonce_base64"),
        CertificadoSubject = r.GetValueOrDefault<string>("certificado_subject"),
        CertificadoValidoAte = r.GetValueOrDefault<DateTime?>("certificado_valido_ate"),
        CertificadoSenhaCriptografada = r.GetValueOrDefault<string>("certificado_senha_criptografada"),
        CertificadoSenhaNonceBase64 = r.GetValueOrDefault<string>("certificado_senha_nonce_base64"),
        CreatedAt = r.GetValueOrDefault<DateTime>("created_at"),
        CreatedBy = r.GetValueOrDefault<Guid?>("created_by"),
        UpdatedAt = r.GetValueOrDefault<DateTime?>("updated_at"),
        UpdatedBy = r.GetValueOrDefault<Guid?>("updated_by"),
        DeletedAt = r.GetValueOrDefault<DateTime?>("deleted_at"),
        DeletedBy = r.GetValueOrDefault<Guid?>("deleted_by")
    };
}
