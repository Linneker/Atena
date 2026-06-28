using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Services.V1.Rh.Funcionario.Query.ListarSalarioVigente;

public sealed record ListarSalarioVigenteQueryResult(
    Guid? HistoricoSalarioId,
    decimal? Valor,
    DateOnly? VigenciaInicio,
    DateOnly? VigenciaFim,
    MotivoSalario? Motivo);
