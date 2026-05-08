using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.ContasReceber.ListarContasReceber;

public sealed record ListarContasReceberRequest(
    StatusConta? Status = null,
    DateTime? VencimentoInicio = null,
    DateTime? VencimentoFim = null,
    Guid? ClienteId = null,
    int? DiasAtrasoMinimo = null,
    int? Skip = null,
    int? Take = null);
