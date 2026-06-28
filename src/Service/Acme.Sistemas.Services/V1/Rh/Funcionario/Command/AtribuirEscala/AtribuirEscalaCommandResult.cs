namespace Acme.Sistemas.Services.V1.Rh.Funcionario.Command.AtribuirEscala;

public sealed record AtribuirEscalaCommandResult(
    Guid EscalaId,
    Guid? VigenciaAnteriorFechadaId);
