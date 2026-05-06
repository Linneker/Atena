using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Estoque.Query.ConsultarSaldo;

public sealed record ConsultarSaldoQuery(Guid ProdutoId, Guid? EstoqueId = null)
    : IRequest<ResponseDefault<ConsultarSaldoQueryResult>>;

public sealed record SaldoPorEstoque(
    Guid EstoqueId, decimal SaldoTotal, decimal SaldoReservado, decimal SaldoDisponivel);

public sealed record ConsultarSaldoQueryResult(
    Guid ProdutoId,
    decimal TotalGeral,
    decimal ReservadoGeral,
    decimal DisponivelGeral,
    IReadOnlyList<SaldoPorEstoque> PorEstoque);
