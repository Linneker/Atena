using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Despesa.Query.ObterDespesa;

public sealed class ObterDespesaQueryHandler
    : IRequestHandler<ObterDespesaQuery, ResponseDefault<ObterDespesaQueryResult>>
{
    private readonly IDespesaRepository _despesas;

    public ObterDespesaQueryHandler(IDespesaRepository despesas)
    {
        _despesas = despesas;
    }

    public async Task<ResponseDefault<ObterDespesaQueryResult>> Handle(
        ObterDespesaQuery request,
        CancellationToken cancellationToken)
    {
        var d = await _despesas.GetByIdAsync(request.Id, cancellationToken);
        if (d is null)
        {
            return ResponseDefault<ObterDespesaQueryResult>.NotFound("Despesa não encontrada.");
        }

        return ResponseDefault<ObterDespesaQueryResult>.Ok(new ObterDespesaQueryResult(
            d.Id, d.Nome, d.Descricao, d.Categoria, d.Valor, d.DespesaFixa,
            d.DataVencimento, d.CompetenciaId, d.CentroDeCustoId, d.FornecedorId,
            d.StatusPagamento, d.ValorPago, d.DataPagamento, d.FormaPagamento,
            d.ObservacaoPagamento, d.CreatedAt, d.UpdatedAt));
    }
}
