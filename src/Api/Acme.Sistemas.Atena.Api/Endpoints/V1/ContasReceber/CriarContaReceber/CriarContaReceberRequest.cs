namespace Acme.Sistemas.Atena.Api.Endpoints.V1.ContasReceber.CriarContaReceber;

public sealed record CriarContaReceberRequest(
    string Descricao,
    Guid? ClienteId,
    Guid? ReceitaId,
    Guid? PlanoDeContasId,
    decimal ValorOriginal,
    DateTime DataVencimento);
