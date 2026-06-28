namespace Acme.Sistemas.Services.V1.Rh.Funcionario.Command.CriarFuncionarioCompleto;

public sealed record CriarFuncionarioCompletoCommandResult(
    Guid FuncionarioId,
    Guid HistoricoSalarioId,
    Guid? EscalaId,
    int BeneficiosCriados,
    int DependentesCriados);
