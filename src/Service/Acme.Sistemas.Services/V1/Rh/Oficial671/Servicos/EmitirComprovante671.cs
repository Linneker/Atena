using Acme.Sistemas.Domain.Entities.Rh;
using Acme.Sistemas.Domain.Entities.Rh.Oficial671;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;
using Acme.Sistemas.Domain.Interfaces.Rh;
using Acme.Sistemas.ExternalIntegration.Sefaz.Certificado;

namespace Acme.Sistemas.Services.V1.Rh.Oficial671.Servicos;

public sealed class EmitirComprovante671 : IEmitirComprovante671
{
    private readonly INumeradorNsr _numerador;
    private readonly IGeradorComprovantePontoTexto _texto;
    private readonly IGeradorComprovantePontoPdf _pdf;
    private readonly IAssinadorComprovante671 _assinador;
    private readonly CertificadoTenantResolver _certResolver;
    private readonly IComprovantePontoRepository _repo;
    private readonly ITenantContext _tenant;

    public EmitirComprovante671(
        INumeradorNsr numerador,
        IGeradorComprovantePontoTexto texto,
        IGeradorComprovantePontoPdf pdf,
        IAssinadorComprovante671 assinador,
        CertificadoTenantResolver certResolver,
        IComprovantePontoRepository repo,
        ITenantContext tenant)
    {
        _numerador = numerador;
        _texto = texto;
        _pdf = pdf;
        _assinador = assinador;
        _certResolver = certResolver;
        _repo = repo;
        _tenant = tenant;
    }

    public async Task<ComprovantePonto> EmitirAsync(
        Guid empresaId, MarcacaoPonto marcacao, DadosFuncionario671 funcionario,
        string cnpjEmpregador, CancellationToken cancellationToken = default)
    {
        var nsr = await _numerador.ProximoAsync(empresaId, cancellationToken);
        var payload = _texto.Gerar(new DadosComprovante671(
            Nsr: nsr,
            TipoRegistro: marcacao.Tipo.ToString(),
            CpfEmpregado: funcionario.Cpf,
            PisEmpregado: funcionario.Pis,
            DataHora: marcacao.DataHora,
            NomeEmpregado: funcionario.NomeCompleto,
            CnpjEmpregador: cnpjEmpregador,
            HashEncadeadoMarcacao: marcacao.HashIntegridade));

        var cert = await _certResolver.GetAsync(cancellationToken);
        var ass = _assinador.Assinar(payload, cert);

        var comprovante = new ComprovantePonto
        {
            TenantId = _tenant.TenantId,
            EmpresaId = empresaId,
            MarcacaoId = marcacao.Id,
            Nsr = nsr,
            PayloadTexto = payload,
            AssinaturaBase64 = ass.AssinaturaBase64,
            HashSha256 = ass.HashSha256Hex,
            CertificadoThumbprint = ass.CertificadoThumbprint,
            EmitidoEm = DateTime.UtcNow,
            CreatedBy = _tenant.UserId,
        };
        await _repo.AddAsync(comprovante, cancellationToken);
        return comprovante;
    }

    public Task<byte[]> GerarPdfAsync(
        ComprovantePonto c, DadosFuncionario671 funcionario,
        DadosEmpregador671 empregador, string? urlVerificacao,
        CancellationToken cancellationToken = default)
    {
        var pdf = _pdf.Gerar(new DadosComprovantePdf(
            RazaoSocialEmpregador: empregador.RazaoSocial,
            CnpjEmpregador: empregador.Cnpj,
            EnderecoEmpregador: empregador.EnderecoCompleto,
            NomeEmpregado: funcionario.NomeCompleto,
            CpfEmpregado: funcionario.Cpf,
            PisEmpregado: funcionario.Pis,
            DataHora: c.EmitidoEm,
            TipoRegistro: "Marcação",
            Nsr: c.Nsr,
            AssinaturaResumoBase64: c.AssinaturaBase64,
            HashSha256Hex: c.HashSha256,
            QrCodeUrlVerificacao: urlVerificacao));
        return Task.FromResult(pdf);
    }
}
