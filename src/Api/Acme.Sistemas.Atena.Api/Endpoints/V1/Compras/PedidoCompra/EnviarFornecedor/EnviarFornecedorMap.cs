using SrvCmd = Acme.Sistemas.Services.V1.PedidoCompra.Command.EnviarFornecedor;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Compras.PedidoCompra.EnviarFornecedor;

public static class EnviarFornecedorMap
{
    public static SrvCmd.EnviarFornecedorCommand ToCommand(this EnviarFornecedorRequest request, Guid pedidoId)
        => new(pedidoId, request.EmailDestinoOverride);

    public static EnviarFornecedorResponse ToResponse(this SrvCmd.EnviarFornecedorCommandResult result)
        => new(result.PedidoId, result.EmailDestino, result.EnviadoEm);
}
