namespace Acme.Sistemas.Domain.Interfaces.Rh;

/// <summary>
/// Gera o payload texto do comprovante de marcação no formato Portaria 671/2021 anexo II,
/// que é uma linha pipe-separated com NSR + tipo + CPF + PIS + data/hora + nome + CNPJ + hash.
/// </summary>
public interface IGeradorComprovantePontoTexto
{
    string Gerar(DadosComprovante671 dados);
}

public sealed record DadosComprovante671(
    long Nsr,
    string TipoRegistro,
    string CpfEmpregado,
    string PisEmpregado,
    DateTime DataHora,
    string NomeEmpregado,
    string CnpjEmpregador,
    string HashEncadeadoMarcacao);
