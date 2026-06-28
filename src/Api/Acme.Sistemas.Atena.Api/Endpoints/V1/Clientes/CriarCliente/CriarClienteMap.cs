using Acme.Sistemas.Services.V1.Cliente.Command.CriarCliente;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Clientes.CriarCliente;

public static class CriarClienteMap
{
    public static CriarClienteCommand ToCommand(this CriarClienteRequest request)
        => new(
            request.Tipo,
            request.Nome,
            request.NomeFantasia,
            request.Documento,
            request.InscricaoEstadual,
            request.Email,
            request.Telefone,
            request.Endereco,
            request.BuscarEnderecoPorCep);

    public static CriarClienteResponse ToResponse(this CriarClienteCommandResult result)
        => new(result.Id, result.Nome, result.Documento);
}
