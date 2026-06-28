using Acme.Sistemas.Services.V1.Rh.Funcionario.Command.RegistrarReajusteSalarial;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Funcionarios.RegistrarReajusteSalarial;

public static class RegistrarReajusteSalarialMap
{
    public static RegistrarReajusteSalarialCommand ToCommand(this RegistrarReajusteSalarialRequest r)
        => new(r.FuncionarioId, r.NovoValor, r.VigenciaInicio, r.Motivo, r.Observacao);

    public static RegistrarReajusteSalarialResponse ToResponse(this RegistrarReajusteSalarialCommandResult r)
        => new(r.HistoricoSalarioId, r.VigenciaAnteriorFechadaId);
}
