namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Vendas.Faturamento.FaturarPedido;

public sealed record FaturarPedidoResponse(
    Guid FaturamentoId,
    string Numero,
    decimal ValorTotal,
    Guid? ContaReceberId,
    Guid? ComissaoId,
    bool NFeSolicitada);
