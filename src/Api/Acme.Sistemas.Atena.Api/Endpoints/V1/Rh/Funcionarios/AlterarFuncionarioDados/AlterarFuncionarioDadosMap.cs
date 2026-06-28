using Acme.Sistemas.Services.V1.Rh.Funcionario.Command.AlterarFuncionarioDados;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Funcionarios.AlterarFuncionarioDados;

public static class AlterarFuncionarioDadosMap
{
    public static AlterarFuncionarioDadosCommand ToCommand(this AlterarFuncionarioDadosRequest r)
        => new(r.Id, r.NomeCompleto, r.Email, r.Telefone,
               r.Rg, r.RgOrgao, r.RgUf, r.EstadoCivil,
               r.Naturalidade, r.Nacionalidade, r.Endereco, r.ContaBancaria);

    public static AlterarFuncionarioDadosResponse ToResponse(this AlterarFuncionarioDadosCommandResult r)
        => new(r.Id);
}
