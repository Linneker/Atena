using Acme.Sistemas.Services.V1.Rh.Funcionario.Command.AlterarFuncionarioContrato;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Funcionarios.AlterarFuncionarioContrato;

public static class AlterarFuncionarioContratoMap
{
    public static AlterarFuncionarioContratoCommand ToCommand(this AlterarFuncionarioContratoRequest r)
        => new(r.Id, r.CargoId, r.LotacaoId, r.DepartamentoId, r.CentroDeCustoId,
               r.TipoContrato, r.RegimeRemuneracao, r.CodigoMatricula,
               r.DataDemissao, r.Status);

    public static AlterarFuncionarioContratoResponse ToResponse(this AlterarFuncionarioContratoCommandResult r)
        => new(r.Id);
}
