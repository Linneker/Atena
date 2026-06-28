using Acme.Sistemas.Services.V1.Empresa.Command.CriarEmpresa;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Empresas.CriarEmpresa;

public static class CriarEmpresaMap
{
    public static CriarEmpresaCommand ToCommand(this CriarEmpresaRequest request)
        => new(
            request.RazaoSocial,
            request.NomeFantasia,
            request.Cnpj,
            request.InscricaoEstadual,
            request.InscricaoMunicipal,
            request.Email,
            request.Telefone,
            request.Endereco,
            request.BuscarEnderecoPorCep);

    public static CriarEmpresaResponse ToResponse(this CriarEmpresaCommandResult result)
        => new(result.Id, result.RazaoSocial, result.Cnpj);
}
