using Acme.Sistemas.Services.V1.Cliente.Command.ExcluirCliente;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Clientes.ExcluirCliente;

public static class ExcluirClienteMap
{
    public static ExcluirClienteCommand ToCommand(this ExcluirClienteRequest request)
        => new(request.Id);
}
