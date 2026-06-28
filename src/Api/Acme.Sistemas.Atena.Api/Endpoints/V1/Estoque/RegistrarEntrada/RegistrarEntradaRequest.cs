using Acme.Sistemas.Domain.Entities.Estoque;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Estoque.RegistrarEntrada;

public sealed record RegistrarEntradaRequest(
    Guid EstoqueId,
    Guid ProdutoId,
    decimal Quantidade,
    decimal? CustoUnitario,
    OrigemMovimento Origem,
    string? Motivo,
    Guid? FornecedorId,
    string? DocumentoReferencia,
    DateTime? DataMovimento);
