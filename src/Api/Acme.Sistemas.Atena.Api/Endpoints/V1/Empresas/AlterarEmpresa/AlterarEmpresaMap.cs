using Acme.Sistemas.Services.V1.Empresa.Command.AlterarEmpresa;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Empresas.AlterarEmpresa;

public static class AlterarEmpresaMap
{
    public static AlterarEmpresaCommand ToCommand(this AlterarEmpresaRequest request, Guid id)
        => new(
            id,
            request.RazaoSocial,
            request.NomeFantasia,
            request.Cnpj,
            request.InscricaoEstadual,
            request.InscricaoMunicipal,
            request.Email,
            request.Telefone,
            request.Status,
            request.Endereco,
            request.BuscarEnderecoPorCep);

    public static AlterarEmpresaResponse ToResponse(this AlterarEmpresaCommandResult result)
        => new(result.Id);
}
