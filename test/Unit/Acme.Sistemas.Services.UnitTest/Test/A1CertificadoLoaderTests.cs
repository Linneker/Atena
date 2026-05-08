using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Acme.Sistemas.Domain.Interfaces.Fiscal;
using Acme.Sistemas.ExternalIntegration.Sefaz.Certificado;
using FluentAssertions;
using Xunit;

namespace Acme.Sistemas.Services.UnitTest.Test;

public class A1CertificadoLoaderTests
{
    private const string Senha = "test-senha-123";

    private static byte[] GerarPfxAutoAssinado(
        DateTimeOffset notBefore,
        DateTimeOffset notAfter,
        bool comDigitalSignature = true,
        string senha = Senha)
    {
        using var rsa = RSA.Create(2048);
        var req = new CertificateRequest(
            "CN=ATENA TESTE PFXSIGN, OU=TESTE, O=TESTE, C=BR",
            rsa,
            HashAlgorithmName.SHA256,
            RSASignaturePadding.Pkcs1);

        if (comDigitalSignature)
        {
            req.CertificateExtensions.Add(new X509KeyUsageExtension(
                X509KeyUsageFlags.DigitalSignature | X509KeyUsageFlags.NonRepudiation,
                critical: false));
        }

        using var cert = req.CreateSelfSigned(notBefore, notAfter);
        return cert.Export(X509ContentType.Pfx, senha);
    }

    [Trait("Solucao", "ExternalIntegration")]
    [Trait("Acao", "A1CertificadoLoader")]
    [Fact(DisplayName = "Dado um PFX válido (cert auto-assinado vigente), quando LoadAsync, então retorna X509Certificate2 com mesmo Subject")]
    public async Task Load_PfxValido_RetornaCert()
    {
        var pfx = GerarPfxAutoAssinado(DateTimeOffset.UtcNow.AddDays(-1), DateTimeOffset.UtcNow.AddYears(1));
        var sut = new A1CertificadoLoader(validarCadeiaIcpBrasil: false);

        var cert = await sut.LoadAsync(pfx, Senha);

        cert.Should().NotBeNull();
        cert.Subject.Should().Contain("ATENA TESTE PFXSIGN");
        cert.HasPrivateKey.Should().BeTrue();
    }

    [Trait("Solucao", "ExternalIntegration")]
    [Trait("Acao", "A1CertificadoLoader")]
    [Fact(DisplayName = "Dado uma senha errada, quando LoadAsync, então lança CertificadoInvalidoException com mensagem clara")]
    public async Task Load_SenhaErrada_LancaCertificadoInvalido()
    {
        var pfx = GerarPfxAutoAssinado(DateTimeOffset.UtcNow.AddDays(-1), DateTimeOffset.UtcNow.AddYears(1));
        var sut = new A1CertificadoLoader(validarCadeiaIcpBrasil: false);

        Func<Task> act = () => sut.LoadAsync(pfx, "senha-errada");

        var ex = await act.Should().ThrowAsync<CertificadoInvalidoException>();
        ex.Which.Message.Should().Contain("senha incorreta").And.Contain("corrompido");
    }

    [Trait("Solucao", "ExternalIntegration")]
    [Trait("Acao", "A1CertificadoLoader")]
    [Fact(DisplayName = "Dado um PFX vencido, quando LoadAsync, então lança CertificadoInvalidoException citando data de vencimento")]
    public async Task Load_CertVencido_Lanca()
    {
        var pfx = GerarPfxAutoAssinado(
            DateTimeOffset.UtcNow.AddYears(-2),
            DateTimeOffset.UtcNow.AddDays(-1));
        var sut = new A1CertificadoLoader(validarCadeiaIcpBrasil: false);

        Func<Task> act = () => sut.LoadAsync(pfx, Senha);

        await act.Should().ThrowAsync<CertificadoInvalidoException>()
            .WithMessage("*vencido*");
    }

    [Trait("Solucao", "ExternalIntegration")]
    [Trait("Acao", "A1CertificadoLoader")]
    [Fact(DisplayName = "Dado um PFX cuja KeyUsage não inclui DigitalSignature, quando LoadAsync, então lança CertificadoInvalidoException")]
    public async Task Load_SemDigitalSignature_Lanca()
    {
        var pfx = GerarPfxAutoAssinado(
            DateTimeOffset.UtcNow.AddDays(-1),
            DateTimeOffset.UtcNow.AddYears(1),
            comDigitalSignature: false);

        // Geramos manualmente um cert apenas com KeyEncipherment para validar o caminho de erro.
        using var rsa = RSA.Create(2048);
        var req = new CertificateRequest("CN=NO-SIGN", rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        req.CertificateExtensions.Add(new X509KeyUsageExtension(X509KeyUsageFlags.KeyEncipherment, critical: false));
        using var cert = req.CreateSelfSigned(DateTimeOffset.UtcNow.AddDays(-1), DateTimeOffset.UtcNow.AddYears(1));
        var pfxNoSign = cert.Export(X509ContentType.Pfx, Senha);

        var sut = new A1CertificadoLoader(validarCadeiaIcpBrasil: false);
        Func<Task> act = () => sut.LoadAsync(pfxNoSign, Senha);

        await act.Should().ThrowAsync<CertificadoInvalidoException>()
            .WithMessage("*DigitalSignature*");
    }

    [Trait("Solucao", "ExternalIntegration")]
    [Trait("Acao", "A1CertificadoLoader")]
    [Fact(DisplayName = "Dado bytes vazios, quando LoadAsync, então lança CertificadoInvalidoException sem chegar a tentar parsear")]
    public async Task Load_BytesVazios_Lanca()
    {
        var sut = new A1CertificadoLoader(validarCadeiaIcpBrasil: false);

        Func<Task> act = () => sut.LoadAsync(Array.Empty<byte>(), Senha);

        await act.Should().ThrowAsync<CertificadoInvalidoException>().WithMessage("*PFX vazio*");
    }
}
