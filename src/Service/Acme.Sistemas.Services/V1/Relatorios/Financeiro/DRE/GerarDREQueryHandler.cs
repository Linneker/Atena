using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Enums;
using Acme.Sistemas.Domain.Interfaces.Repository;
using PlanoEntity = Acme.Sistemas.Domain.Entities.Financeiro.PlanoDeContas;

namespace Acme.Sistemas.Services.V1.Relatorios.Financeiro.DRE;

public sealed class GerarDREQueryHandler
    : IRequestHandler<GerarDREQuery, ResponseDefault<DREResult>>
{
    private readonly IPlanoDeContasRepository _planos;
    private readonly IRelatoriosFinanceirosRepository _agg;

    public GerarDREQueryHandler(
        IPlanoDeContasRepository planos,
        IRelatoriosFinanceirosRepository agg)
    {
        _planos = planos;
        _agg = agg;
    }

    public async Task<ResponseDefault<DREResult>> Handle(GerarDREQuery request, CancellationToken cancellationToken)
    {
        var planos = await _planos.ListAllAsync(cancellationToken);
        var receitasAgg = await _agg.AggregateContasReceberPorPlanoAsync(request.Inicio, request.Fim, cancellationToken);
        var despesasAgg = await _agg.AggregateContasPagarPorPlanoAsync(request.Inicio, request.Fim, cancellationToken);

        var receitasTree = BuildTree(planos, receitasAgg, TipoConta.Receita);
        var despesasTree = BuildTree(planos, despesasAgg, TipoConta.Despesa);

        var totalReceitas = receitasTree.Sum(l => l.Total);
        var totalDespesas = despesasTree.Sum(l => l.Total);

        return ResponseDefault<DREResult>.Ok(new DREResult(
            request.Inicio, request.Fim,
            receitasTree, despesasTree,
            totalReceitas, totalDespesas,
            totalReceitas - totalDespesas));
    }

    private static IReadOnlyList<DRELinha> BuildTree(
        IReadOnlyList<PlanoEntity> planos,
        IReadOnlyDictionary<Guid, decimal> aggregations,
        TipoConta tipoFiltro)
    {
        var filtrados = planos.Where(p => p.Tipo == tipoFiltro && p.Ativo).ToList();
        var nodes = filtrados.ToDictionary(
            p => p.Id,
            p => new DRELinha
            {
                PlanoId = p.Id,
                Codigo = p.Codigo,
                Nome = p.Nome,
                Nivel = p.Nivel,
                Valor = aggregations.GetValueOrDefault(p.Id, 0m)
            });

        var raiz = new List<DRELinha>();
        foreach (var p in filtrados.OrderBy(x => x.Codigo))
        {
            var node = nodes[p.Id];
            if (p.PaiId.HasValue && nodes.TryGetValue(p.PaiId.Value, out var pai))
                pai.Filhos.Add(node);
            else
                raiz.Add(node);
        }

        foreach (var node in raiz) ConsolidarRecursivo(node);
        return raiz;
    }

    private static decimal ConsolidarRecursivo(DRELinha node)
    {
        node.Total = node.Valor + node.Filhos.Sum(ConsolidarRecursivo);
        return node.Total;
    }
}
