using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Interfaces.Repository;

namespace Acme.Sistemas.Services.V1.Dashboard.Query.EvolucaoFinanceira;

public sealed class EvolucaoFinanceiraQueryHandler
    : IRequestHandler<EvolucaoFinanceiraQuery, ResponseDefault<EvolucaoFinanceiraQueryResult>>
{
    private readonly IDashboardRepository _repo;

    public EvolucaoFinanceiraQueryHandler(IDashboardRepository repo) => _repo = repo;

    public async Task<ResponseDefault<EvolucaoFinanceiraQueryResult>> Handle(EvolucaoFinanceiraQuery request, CancellationToken cancellationToken)
    {
        var raw = await _repo.EvolucaoFinanceiraUltimosMesesAsync(request.Meses, cancellationToken);

        // Preenche meses sem movimento com zero
        var hoje = DateTime.UtcNow;
        var pontos = new List<EvolucaoMesItem>(request.Meses);
        for (int i = request.Meses - 1; i >= 0; i--)
        {
            var d = hoje.AddMonths(-i);
            var match = raw.FirstOrDefault(x => x.Ano == d.Year && x.Mes == d.Month);
            pontos.Add(new EvolucaoMesItem(d.Year, d.Month,
                match.Receitas, match.Despesas,
                match.Receitas - match.Despesas));
        }

        return ResponseDefault<EvolucaoFinanceiraQueryResult>.Ok(new EvolucaoFinanceiraQueryResult(
            request.Meses,
            pontos,
            pontos.Sum(p => p.Receitas),
            pontos.Sum(p => p.Despesas)));
    }
}
