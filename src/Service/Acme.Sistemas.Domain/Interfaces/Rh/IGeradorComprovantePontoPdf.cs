namespace Acme.Sistemas.Domain.Interfaces.Rh;

/// <summary>
/// Gera PDF do comprovante de marcação para 1ª via instantânea + 2ª via sob demanda.
/// Determinístico: mesmo input → mesmos bytes (sem timestamp dinâmico no doc).
/// </summary>
public interface IGeradorComprovantePontoPdf
{
    byte[] Gerar(DadosComprovantePdf dados);
}

public sealed record DadosComprovantePdf(
    string RazaoSocialEmpregador,
    string CnpjEmpregador,
    string EnderecoEmpregador,
    string NomeEmpregado,
    string CpfEmpregado,
    string PisEmpregado,
    DateTime DataHora,
    string TipoRegistro,
    long Nsr,
    string AssinaturaResumoBase64,
    string HashSha256Hex,
    string? QrCodeUrlVerificacao);
