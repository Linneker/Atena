namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Funcionarios.RemoverBeneficio;

public sealed record RemoverBeneficioRequest(Guid FuncionarioId, Guid VinculoId);
