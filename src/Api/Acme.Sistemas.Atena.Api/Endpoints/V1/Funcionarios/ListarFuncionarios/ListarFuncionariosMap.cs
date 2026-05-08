using Acme.Sistemas.Services.V1.Funcionario.Query.ListarFuncionarios;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Funcionarios.ListarFuncionarios;

public static class ListarFuncionariosMap
{
    public static ListarFuncionariosQuery ToQuery(this ListarFuncionariosRequest request)
        => new(request.Skip, request.Take);

    public static ListarFuncionariosResponse ToResponse(this ListarFuncionariosQueryResult result)
        => new(result.Items.Select(i => i.ToResponseItem()).ToArray());

    private static ListarFuncionariosResponseItem ToResponseItem(this ListarFuncionariosQueryItem item)
        => new(item.Id, item.NomeCompleto, item.Cpf, item.Email, item.Cargo, item.Departamento,
            item.CentroDeCustoId, item.DataAdmissao, item.DataDemissao, item.Status);
}
