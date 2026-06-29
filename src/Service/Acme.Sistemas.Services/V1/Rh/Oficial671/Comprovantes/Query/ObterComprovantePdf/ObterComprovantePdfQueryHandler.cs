using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;
using Acme.Sistemas.Domain.Interfaces.Repository.Rh;
using Acme.Sistemas.Services.V1.Rh.Oficial671.Servicos;

namespace Acme.Sistemas.Services.V1.Rh.Oficial671.Comprovantes.Query.ObterComprovantePdf;

/// <summary>
/// 2ª via determinística do PDF do comprovante: regenera a partir do <c>ComprovantePonto</c>
/// persistido + dados do funcionário/empresa atuais. Não muda assinatura — apenas re-render.
/// </summary>
public sealed class ObterComprovantePdfQueryHandler
    : IRequestHandler<ObterComprovantePdfQuery, ResponseDefault<ObterComprovantePdfQueryResult>>
{
    private readonly IComprovantePontoRepository _comprovantes;
    private readonly IMarcacaoPontoRepository _marcacoes;
    private readonly IFuncionarioRepository _funcionarios;
    private readonly IEmpresaRepository _empresas;
    private readonly IEmitirComprovante671 _emit;

    public ObterComprovantePdfQueryHandler(
        IComprovantePontoRepository comprovantes,
        IMarcacaoPontoRepository marcacoes,
        IFuncionarioRepository funcionarios,
        IEmpresaRepository empresas,
        IEmitirComprovante671 emit)
    {
        _comprovantes = comprovantes;
        _marcacoes = marcacoes;
        _funcionarios = funcionarios;
        _empresas = empresas;
        _emit = emit;
    }

    public async Task<ResponseDefault<ObterComprovantePdfQueryResult>> Handle(
        ObterComprovantePdfQuery q, CancellationToken cancellationToken)
    {
        var comprovante = await _comprovantes.GetByMarcacaoAsync(q.MarcacaoId, cancellationToken);
        if (comprovante is null)
            return ResponseDefault<ObterComprovantePdfQueryResult>.NotFound(
                "Comprovante não encontrado para esta marcação.");

        var marcacao = await _marcacoes.GetByIdAsync(q.MarcacaoId, cancellationToken)
            ?? throw new InvalidOperationException("Marcação órfã — comprovante sem marcação.");
        var func = await _funcionarios.GetByIdAsync(marcacao.FuncionarioId, cancellationToken);
        var empresa = await _empresas.GetByIdAsync(comprovante.EmpresaId, cancellationToken)
            ?? throw new InvalidOperationException("Empresa do comprovante não localizada.");

        var endereco = string.Join(", ", new[]
        {
            empresa.Endereco.Logradouro, empresa.Endereco.Numero,
            empresa.Endereco.Bairro, empresa.Endereco.Cidade, empresa.Endereco.Uf,
        }.Where(s => !string.IsNullOrWhiteSpace(s)));

        var pdf = await _emit.GerarPdfAsync(
            comprovante,
            new DadosFuncionario671(
                NomeCompleto: func?.NomeCompleto ?? "Funcionário",
                Cpf: func?.Cpf ?? string.Empty,
                Pis: func?.Pis ?? string.Empty),
            new DadosEmpregador671(empresa.RazaoSocial, empresa.Cnpj, endereco),
            urlVerificacao: null,
            cancellationToken);

        return ResponseDefault<ObterComprovantePdfQueryResult>.Ok(
            new ObterComprovantePdfQueryResult(pdf, $"comprovante-{comprovante.Nsr:D9}.pdf"));
    }
}
