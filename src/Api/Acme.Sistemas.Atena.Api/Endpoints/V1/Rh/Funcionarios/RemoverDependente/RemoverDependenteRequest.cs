namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Funcionarios.RemoverDependente;

public sealed record RemoverDependenteRequest(Guid FuncionarioId, Guid DependenteId);
