using Acme.Sistemas.Domain.Entities.Rh;
using Acme.Sistemas.Domain.Entities.Rh.Oficial671;

namespace Acme.Sistemas.Services.V1.Rh.Oficial671.Servicos;

/// <summary>
/// Orquestrador da emissão do comprovante 671 anexo II: reserva NSR atômico, monta
/// payload texto, assina com cert ICP-Brasil do tenant, persiste <c>ComprovantePonto</c>.
/// </summary>
public interface IEmitirComprovante671
{
    Task<ComprovantePonto> EmitirAsync(
        Guid empresaId,
        MarcacaoPonto marcacao,
        DadosFuncionario671 funcionario,
        string cnpjEmpregador,
        CancellationToken cancellationToken = default);

    /// <summary>Regenera o PDF a partir de um comprovante já persistido (2ª via).</summary>
    Task<byte[]> GerarPdfAsync(
        ComprovantePonto comprovante,
        DadosFuncionario671 funcionario,
        DadosEmpregador671 empregador,
        string? urlVerificacao,
        CancellationToken cancellationToken = default);
}

public sealed record DadosFuncionario671(
    string NomeCompleto, string Cpf, string Pis);

public sealed record DadosEmpregador671(
    string RazaoSocial, string Cnpj, string EnderecoCompleto);
