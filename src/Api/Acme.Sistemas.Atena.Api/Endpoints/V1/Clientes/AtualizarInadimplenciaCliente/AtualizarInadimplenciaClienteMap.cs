using Acme.Sistemas.Services.V1.Cliente.Command.AtualizarInadimplencia;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Clientes.AtualizarInadimplenciaCliente;

public static class AtualizarInadimplenciaClienteMap
{
    public static AtualizarInadimplenciaCommand ToCommand(this AtualizarInadimplenciaClienteRequest request, Guid id)
        => new(id, request.Inadimplente, request.BloquearVendas);

    public static AtualizarInadimplenciaClienteResponse ToResponse(this AtualizarInadimplenciaCommandResult result)
        => new(result.Id, result.Inadimplente, result.BloqueadoVendas);
}
