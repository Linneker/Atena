using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.ContasPagar.ListarContasPagar;

public sealed record ListarContasPagarRequest(
    StatusConta? Status = null,
    DateTime? VencimentoInicio = null,
    DateTime? VencimentoFim = null,
    Guid? FornecedorId = null,
    bool? VencendoEmAteSeteDias = null,
    int? Skip = null,
    int? Take = null);
