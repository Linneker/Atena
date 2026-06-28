namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Compras.SolicitacaoCompra.RejeitarSolicitacao;

public sealed record RejeitarSolicitacaoResponse(
    Guid Id,
    DateTime RejeitadoEm);
