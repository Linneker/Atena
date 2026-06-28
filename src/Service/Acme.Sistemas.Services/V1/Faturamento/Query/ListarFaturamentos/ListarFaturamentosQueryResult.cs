using Acme.Sistemas.Domain.Entities.Vendas;

namespace Acme.Sistemas.Services.V1.Faturamento.Query.ListarFaturamentos;

public sealed record ListarFaturamentosQueryItem(
    Guid Id,
    string Numero,
    Guid PedidoVendaId,
    DateTime DataFaturamento,
    TipoFaturamento Tipo,
    decimal ValorTotal,
    Guid? NFeId,
    Guid? ContaReceberId);

public sealed record ListarFaturamentosQueryResult(
    IReadOnlyList<ListarFaturamentosQueryItem> Items,
    long Total,
    int Skip,
    int Take);
