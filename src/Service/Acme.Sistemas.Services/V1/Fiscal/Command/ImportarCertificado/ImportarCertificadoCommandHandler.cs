using System.Security.Cryptography.X509Certificates;
using System.Text;
using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Core.Security;
using Acme.Sistemas.Domain.Entities.Fiscal;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Fiscal.Command.ImportarCertificado;

public sealed class ImportarCertificadoCommandHandler
    : IRequestHandler<ImportarCertificadoCommand, ResponseDefault<ImportarCertificadoCommandResult>>
{
    private readonly IConfiguracaoFiscalRepository _repo;
    private readonly TenantSecretCipher _cipher;
    private readonly ITenantContext _tenantContext;

    public ImportarCertificadoCommandHandler(
        IConfiguracaoFiscalRepository repo,
        TenantSecretCipher cipher,
        ITenantContext tenantContext)
    {
        _repo = repo;
        _cipher = cipher;
        _tenantContext = tenantContext;
    }

    public async Task<ResponseDefault<ImportarCertificadoCommandResult>> Handle(ImportarCertificadoCommand request, CancellationToken cancellationToken)
    {
        X509Certificate2 cert;
        try
        {
            cert = new X509Certificate2(
                request.PfxConteudo, request.Senha,
                X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet | X509KeyStorageFlags.Exportable);
        }
        catch (Exception ex)
        {
            return ResponseDefault<ImportarCertificadoCommandResult>.BadRequest(
                Core.Response.Erros.Error.Validation(
                    $"Falha ao abrir PFX: {ex.Message}. Verifique a senha e o arquivo."));
        }

        var (cipherPfx, noncePfx) = _cipher.Encrypt(request.PfxConteudo, _tenantContext.TenantId);
        var (cipherSenha, nonceSenha) = _cipher.Encrypt(Encoding.UTF8.GetBytes(request.Senha), _tenantContext.TenantId);

        var config = await _repo.GetAsync(cancellationToken) ?? new ConfiguracaoFiscal
        {
            TenantId = _tenantContext.TenantId,
            CnpjEmitente = string.Empty
        };

        config.CertificadoPfxCriptografado = cipherPfx;
        config.CertificadoNonceBase64 = noncePfx;
        config.CertificadoSenhaCriptografada = Convert.ToBase64String(cipherSenha);
        config.CertificadoSenhaNonceBase64 = nonceSenha;
        config.CertificadoSubject = cert.Subject;
        config.CertificadoValidoAte = cert.NotAfter.ToUniversalTime();
        config.UpdatedBy = _tenantContext.UserId;

        await _repo.UpsertAsync(config, cancellationToken);

        var dias = (int)(cert.NotAfter.ToUniversalTime() - DateTime.UtcNow).TotalDays;
        return ResponseDefault<ImportarCertificadoCommandResult>.Ok(
            new ImportarCertificadoCommandResult(cert.Subject, cert.NotAfter.ToUniversalTime(), dias));
    }
}
