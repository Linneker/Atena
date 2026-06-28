namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Funcionarios.ObterSalarioVigente;

public sealed record ObterSalarioVigenteRequest(Guid FuncionarioId, DateOnly Em);
