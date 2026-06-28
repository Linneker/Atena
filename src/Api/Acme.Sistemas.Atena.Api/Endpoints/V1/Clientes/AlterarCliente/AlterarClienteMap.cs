using Acme.Sistemas.Services.V1.Cliente.Command.AlterarCliente;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Clientes.AlterarCliente;

public static class AlterarClienteMap
{
    public static AlterarClienteCommand ToCommand(this AlterarClienteRequest request, Guid id)
        => new(
            id,
            request.Tipo,
            request.Nome,
            request.NomeFantasia,
            request.Documento,
            request.InscricaoEstadual,
            request.Email,
            request.Telefone,
            request.Status,
            request.Endereco);

    public static AlterarClienteResponse ToResponse(this AlterarClienteCommandResult result)
        => new(result.Id);
}
