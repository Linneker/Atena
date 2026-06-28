using Acme.Sistemas.Domain.Entities.Compras;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Compras.SolicitacaoCompra.ListarSolicitacoes;

public sealed record ListarSolicitacoesRequest(
    StatusSolicitacaoCompra? Status = null,
    int Skip = 0,
    int Take = 50);
