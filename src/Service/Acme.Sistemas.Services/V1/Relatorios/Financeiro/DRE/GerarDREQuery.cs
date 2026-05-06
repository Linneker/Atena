using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Relatorios.Financeiro.DRE;

public sealed record GerarDREQuery(DateTime Inicio, DateTime Fim) : IRequest<ResponseDefault<DREResult>>;

public sealed class DRELinha
{
    public Guid PlanoId { get; init; }
    public string Codigo { get; init; } = string.Empty;
    public string Nome { get; init; } = string.Empty;
    public int Nivel { get; init; }
    public decimal Valor { get; set; }
    public decimal Total { get; set; }
    public List<DRELinha> Filhos { get; init; } = new();
}

public sealed record DREResult(
    DateTime Inicio,
    DateTime Fim,
    IReadOnlyList<DRELinha> Receitas,
    IReadOnlyList<DRELinha> Despesas,
    decimal TotalReceitas,
    decimal TotalDespesas,
    decimal ResultadoLiquido);
