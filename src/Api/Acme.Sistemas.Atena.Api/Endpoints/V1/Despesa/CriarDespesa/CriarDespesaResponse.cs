namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Despesa.CriarDespesa;

public sealed record CriarDespesaResponse(
    Guid Id,
    string Nome,
    decimal Valor,
    DateTime DataVencimento);
