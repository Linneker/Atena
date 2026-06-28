namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Compras.PedidoCompra.EnviarFornecedor;

public sealed record EnviarFornecedorResponse(
    Guid PedidoId,
    string EmailDestino,
    DateTime EnviadoEm);
