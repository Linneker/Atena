namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Vendas.DevolucaoVenda.RegistrarDevolucao;

public sealed record RegistrarDevolucaoRequestItem(
    Guid FaturamentoItemId,
    decimal Quantidade);

public sealed record RegistrarDevolucaoRequest(
    Guid FaturamentoId,
    Guid EstoqueDestinoId,
    string? Motivo,
    IReadOnlyList<RegistrarDevolucaoRequestItem> Itens);
