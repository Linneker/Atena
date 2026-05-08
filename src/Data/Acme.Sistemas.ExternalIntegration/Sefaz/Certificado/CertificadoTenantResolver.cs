using System.Collections.Concurrent;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Acme.Sistemas.Core;
using Acme.Sistemas.Core.Security;
using Acme.Sistemas.Domain.Interfaces.Fiscal;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.ExternalIntegration.Sefaz.Certificado;

/// <summary>
/// Resolve o certificado A1 de um tenant: lê `ConfiguracaoFiscal`, descriptografa a senha
/// via <see cref="TenantSecretCipher"/>, delega o load ao <see cref="ICertificadoLoader"/>,
/// e cacheia o `X509Certificate2` em memória até a véspera do vencimento.
///
/// Cache é por (tenantId), não por (tenantId, ambiente). Trocar de ambiente não invalida
/// o cert — ambiente afeta URLs SEFAZ, não o cert em si.
/// </summary>
public sealed class CertificadoTenantResolver
{
    private readonly IConfiguracaoFiscalRepository _config;
    private readonly TenantSecretCipher _cipher;
    private readonly ICertificadoLoader _loader;
    private readonly ITenantContext _tenant;
    private readonly ConcurrentDictionary<Guid, CacheEntry> _cache = new();
    private readonly TimeSpan _margemAntesVencimento;

    public CertificadoTenantResolver(
        IConfiguracaoFiscalRepository config,
        TenantSecretCipher cipher,
        ICertificadoLoader loader,
        ITenantContext tenant,
        TimeSpan? margemAntesVencimento = null)
    {
        _config = config;
        _cipher = cipher;
        _loader = loader;
        _tenant = tenant;
        _margemAntesVencimento = margemAntesVencimento ?? TimeSpan.FromDays(1);
    }

    /// <summary>
    /// Retorna o cert do tenant atual. Idempotente — múltiplas chamadas retornam a mesma instância
    /// cacheada até a expiração da entrada.
    /// </summary>
    public async Task<X509Certificate2> GetAsync(CancellationToken cancellationToken = default)
    {
        var tenantId = _tenant.TenantId;
        if (tenantId == Guid.Empty)
            throw new InvalidOperationException("TenantContext não autenticado — não dá pra resolver certificado fiscal.");

        if (_cache.TryGetValue(tenantId, out var entry) && entry.ValidaAte > DateTime.UtcNow)
            return entry.Cert;

        var fresh = await CarregarAsync(tenantId, cancellationToken);
        var validaAte = fresh.NotAfter.ToUniversalTime() - _margemAntesVencimento;
        _cache[tenantId] = new CacheEntry(fresh, validaAte);
        return fresh;
    }

    /// <summary>Invalida cache do tenant atual — útil após upload de novo PFX.</summary>
    public void Invalidar()
    {
        if (_cache.TryRemove(_tenant.TenantId, out var entry))
            entry.Cert.Dispose();
    }

    private async Task<X509Certificate2> CarregarAsync(Guid tenantId, CancellationToken ct)
    {
        var config = await _config.GetAsync(ct)
            ?? throw new InvalidOperationException("ConfiguracaoFiscal não encontrada para o tenant.");

        if (config.CertificadoPfxCriptografado is null || config.CertificadoPfxCriptografado.Length == 0)
            throw new InvalidOperationException("Certificado PFX não configurado para o tenant — fazer upload primeiro.");

        if (string.IsNullOrEmpty(config.CertificadoSenhaCriptografada) || string.IsNullOrEmpty(config.CertificadoSenhaNonceBase64))
            throw new InvalidOperationException("Senha do certificado não armazenada para o tenant.");

        // Senha está em Base64 (cipher) + Nonce em Base64 separados. PFX em bytes diretos.
        // O PFX em si NÃO é criptografado em camada extra — a senha PKCS12 já protege.
        // (Caso futuro: criptografar o blob também — não é o caso hoje.)
        var senhaCipher = Convert.FromBase64String(config.CertificadoSenhaCriptografada!);
        var senhaPlain = _cipher.Decrypt(senhaCipher, config.CertificadoSenhaNonceBase64!, tenantId);
        var senha = Encoding.UTF8.GetString(senhaPlain);

        return await _loader.LoadAsync(config.CertificadoPfxCriptografado, senha, ct);
    }

    private sealed record CacheEntry(X509Certificate2 Cert, DateTime ValidaAte);
}
