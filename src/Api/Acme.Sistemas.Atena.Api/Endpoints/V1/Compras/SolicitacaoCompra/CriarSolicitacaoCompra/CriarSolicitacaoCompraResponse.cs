namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Compras.SolicitacaoCompra.CriarSolicitacaoCompra;

public sealed record CriarSolicitacaoCompraResponse(
    Guid Id,
    string Numero,
    decimal ValorTotal);
