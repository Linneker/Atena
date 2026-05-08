namespace Acme.Sistemas.Atena.Api.Endpoints.V1.ContasPagar.CriarContaPagar;

public sealed record CriarContaPagarResponse(
    Guid Id,
    string Descricao,
    decimal ValorOriginal,
    DateTime DataVencimento);
