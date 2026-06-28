using Acme.Sistemas.Services.V1.Rh.Funcionario.Command.CadastrarDependente;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Funcionarios.CadastrarDependente;

public static class CadastrarDependenteMap
{
    public static CadastrarDependenteCommand ToCommand(this CadastrarDependenteRequest r)
        => new(r.FuncionarioId, r.NomeCompleto, r.Cpf, r.DataNascimento, r.Tipo,
               r.Irrf, r.SalarioFamilia, r.PensaoAlimenticiaPct);

    public static CadastrarDependenteResponse ToResponse(this CadastrarDependenteCommandResult r)
        => new(r.Id);
}
