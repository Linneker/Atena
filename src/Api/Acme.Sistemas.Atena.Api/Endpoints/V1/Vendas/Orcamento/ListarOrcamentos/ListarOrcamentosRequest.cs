using Acme.Sistemas.Domain.Entities.Vendas;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Vendas.Orcamento.ListarOrcamentos;

public sealed record ListarOrcamentosRequest(
    StatusOrcamento? Status = null,
    Guid? ClienteId = null,
    int Skip = 0,
    int Take = 50);
