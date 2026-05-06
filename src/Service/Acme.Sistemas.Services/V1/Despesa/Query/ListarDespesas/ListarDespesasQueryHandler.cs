using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Despesa.Query.ListarDespesas;

public sealed class ListarDespesasQueryHandler
    : IRequestHandler<ListarDespesasQuery, ResponseDefault<ListarDespesasQueryResult>>
{
    private readonly IDespesaRepository _despesas;

    public ListarDespesasQueryHandler(IDespesaRepository despesas)
    {
        _despesas = despesas;
    }

    public async Task<ResponseDefault<ListarDespesasQueryResult>> Handle(
        ListarDespesasQuery request,
        CancellationToken cancellationToken)
    {
        var despesas = await _despesas.ListByFiltroAsync(
            request.Status, request.VencimentoInicio, request.VencimentoFim,
            request.Categoria, request.CompetenciaId,
            request.Skip, request.Take, cancellationToken);

        var total = await _despesas.CountByFiltroAsync(
            request.Status, request.VencimentoInicio, request.VencimentoFim,
            request.Categoria, request.CompetenciaId, cancellationToken);

        var items = despesas.Select(d => new ListarDespesasQueryItem(
            d.Id, d.Nome, d.Categoria, d.Valor, d.DataVencimento,
            d.StatusPagamento, d.ValorPago, d.DataPagamento,
            d.CompetenciaId, d.CentroDeCustoId, d.FornecedorId, d.DespesaFixa)).ToList();

        return ResponseDefault<ListarDespesasQueryResult>.Ok(
            new ListarDespesasQueryResult(items, total, request.Skip, request.Take));
    }
}
