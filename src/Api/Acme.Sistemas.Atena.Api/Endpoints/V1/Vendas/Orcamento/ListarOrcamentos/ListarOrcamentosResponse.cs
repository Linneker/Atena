using Acme.Sistemas.Domain.Entities.Vendas;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Vendas.Orcamento.ListarOrcamentos;

public sealed record ListarOrcamentosResponseItem(
    Guid Id,
    string Numero,
    Guid ClienteId,
    Guid? VendedorId,
    DateTime DataEmissao,
    DateTime DataValidade,
    decimal ValorTotal,
    StatusOrcamento Status);

public sealed record ListarOrcamentosResponse(
    IReadOnlyList<ListarOrcamentosResponseItem> Items,
    long Total);
