namespace Acme.Sistemas.Services.V1.Rh.Funcionario.Command.RegistrarReajusteSalarial;

public sealed record RegistrarReajusteSalarialCommandResult(
    Guid HistoricoSalarioId,
    Guid? VigenciaAnteriorFechadaId);
