namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Vendas.DevolucaoVenda.RegistrarDevolucao;

public sealed record RegistrarDevolucaoResponse(
    Guid DevolucaoId,
    decimal ValorDevolvido,
    bool ContaReceberEstornada,
    bool NFeDevolucaoSolicitada);
