namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Receita.CriarReceita;

public sealed record CriarReceitaResponse(
    Guid Id,
    string Nome,
    decimal Valor,
    DateTime DataPrevistaRecebimento);
