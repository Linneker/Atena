using Acme.Sistemas.Services.V1.Rh.Funcionario.Command.AtribuirEscala;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Funcionarios.AtribuirEscala;

public static class AtribuirEscalaMap
{
    public static AtribuirEscalaCommand ToCommand(this AtribuirEscalaRequest r)
        => new(r.FuncionarioId, r.JornadaId, r.VigenciaInicio, r.Observacao);

    public static AtribuirEscalaResponse ToResponse(this AtribuirEscalaCommandResult r)
        => new(r.EscalaId, r.VigenciaAnteriorFechadaId);
}
