using Acme.Sistemas.Domain.Entities.Compras;
using PedidoCompraEntity = Acme.Sistemas.Domain.Entities.Compras.PedidoCompra;

namespace Acme.Sistemas.Domain.Interfaces.Reports;

public sealed record PedidoCompraPdfData(
    PedidoCompraEntity Pedido,
    IReadOnlyList<PedidoCompraItem> Itens,
    string FornecedorNome,
    string? FornecedorEmail);

public interface IPedidoCompraPdfRenderer
{
    byte[] Render(PedidoCompraPdfData data, TenantBranding branding);
}
