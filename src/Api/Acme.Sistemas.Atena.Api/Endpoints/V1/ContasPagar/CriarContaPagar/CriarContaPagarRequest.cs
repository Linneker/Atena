namespace Acme.Sistemas.Atena.Api.Endpoints.V1.ContasPagar.CriarContaPagar;

public sealed record CriarContaPagarRequest(
    string Descricao,
    Guid? FornecedorId,
    Guid? DespesaId,
    Guid? PlanoDeContasId,
    decimal ValorOriginal,
    DateTime DataVencimento,
    string? Observacao);
