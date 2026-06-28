namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Funcionarios.RegistrarReajusteSalarial;

public sealed record RegistrarReajusteSalarialResponse(
    Guid HistoricoSalarioId,
    Guid? VigenciaAnteriorFechadaId);
