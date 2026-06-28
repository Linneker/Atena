namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Funcionarios.CriarFuncionarioCompleto;

public sealed record CriarFuncionarioCompletoResponse(
    Guid FuncionarioId,
    Guid HistoricoSalarioId,
    Guid? EscalaId,
    int BeneficiosCriados,
    int DependentesCriados);
