using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Relatorios.Vendas;

public sealed class RelatorioVendasQueryHandler
    : IRequestHandler<RelatorioVendasQuery, ResponseDefault<RelatorioVendasResult>>
{
    private readonly IRelatoriosVendasRepository _repo;

    public RelatorioVendasQueryHandler(IRelatoriosVendasRepository repo) => _repo = repo;

    public async Task<ResponseDefault<RelatorioVendasResult>> Handle(RelatorioVendasQuery request, CancellationToken cancellationToken)
    {
        IReadOnlyList<RelatorioVendasLinha> linhas;

        switch (request.Agrupamento)
        {
            case AgrupamentoVendas.Vendedor:
                var v = await _repo.AgruparPorVendedorAsync(request.Inicio, request.Fim, cancellationToken);
                linhas = v.Select(x => new RelatorioVendasLinha(x.VendedorId, null, x.Total, x.Faturamentos)).ToList();
                break;

            case AgrupamentoVendas.Cliente:
                var c = await _repo.AgruparPorClienteAsync(request.Inicio, request.Fim, cancellationToken);
                linhas = c.Select(x => new RelatorioVendasLinha(x.ClienteId, null, x.Total, x.Faturamentos)).ToList();
                break;

            case AgrupamentoVendas.Produto:
                var p = await _repo.AgruparPorProdutoAsync(request.Inicio, request.Fim, cancellationToken);
                linhas = p.Select(x => new RelatorioVendasLinha(x.ProdutoId, x.Quantidade, x.Total, null)).ToList();
                break;

            default:
                linhas = Array.Empty<RelatorioVendasLinha>();
                break;
        }

        var totalGeral = linhas.Sum(l => l.Total);
        return ResponseDefault<RelatorioVendasResult>.Ok(
            new RelatorioVendasResult(request.Inicio, request.Fim, request.Agrupamento, totalGeral, linhas));
    }
}
