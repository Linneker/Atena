using Acme.Sistemas.Services.V1.RecebimentoCompra.Command.RegistrarRecebimento;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Compras.RecebimentoCompra.RegistrarRecebimento;

public static class RegistrarRecebimentoMap
{
    public static RegistrarRecebimentoCommand ToCommand(this RegistrarRecebimentoRequest request)
        => new(
            request.PedidoCompraId,
            request.EstoqueId,
            request.DataRecebimento,
            request.NumeroNotaFiscal,
            request.ChaveAcessoNFe,
            request.Observacao,
            request.VencimentoContaPagar,
            request.PlanoDeContasId,
            request.Itens.Select(i => new RecebimentoItemDto(i.PedidoCompraItemId, i.QuantidadeRecebida, i.PrecoUnitario, i.Observacao)).ToArray());

    public static RegistrarRecebimentoResponse ToResponse(this RegistrarRecebimentoCommandResult result)
        => new(result.RecebimentoId, result.Tipo, result.ContaPagarId, result.ValorTotalRecebido, result.EntradasGeradas);
}
