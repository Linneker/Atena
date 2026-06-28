using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Receita.ReceberReceita;

public sealed record ReceberReceitaRequest(
    decimal ValorRecebido,
    DateTime DataRecebimento,
    FormaPagamento FormaPagamento,
    string? Observacao);
