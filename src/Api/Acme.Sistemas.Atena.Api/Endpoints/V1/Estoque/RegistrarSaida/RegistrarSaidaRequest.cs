using Acme.Sistemas.Domain.Entities.Estoque;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Estoque.RegistrarSaida;

public sealed record RegistrarSaidaRequest(
    Guid EstoqueId,
    Guid ProdutoId,
    decimal Quantidade,
    decimal? CustoUnitario,
    OrigemMovimento Origem,
    string? Motivo,
    Guid? ClienteId,
    string? DocumentoReferencia,
    DateTime? DataMovimento);
