using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Funcionarios.ObterSalarioVigente;

public sealed record ObterSalarioVigenteResponse(
    Guid? HistoricoSalarioId,
    decimal? Valor,
    DateOnly? VigenciaInicio,
    DateOnly? VigenciaFim,
    MotivoSalario? Motivo);
