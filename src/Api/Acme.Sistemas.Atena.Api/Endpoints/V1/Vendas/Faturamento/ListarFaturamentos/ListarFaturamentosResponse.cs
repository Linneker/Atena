using Acme.Sistemas.Domain.Entities.Vendas;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Vendas.Faturamento.ListarFaturamentos;

public sealed record ListarFaturamentosResponseItem(
    Guid Id,
    string Numero,
    Guid PedidoVendaId,
    DateTime DataFaturamento,
    TipoFaturamento Tipo,
    decimal ValorTotal,
    Guid? NFeId,
    Guid? ContaReceberId);

public sealed record ListarFaturamentosResponse(
    IReadOnlyList<ListarFaturamentosResponseItem> Items,
    long Total,
    int Skip,
    int Take);
