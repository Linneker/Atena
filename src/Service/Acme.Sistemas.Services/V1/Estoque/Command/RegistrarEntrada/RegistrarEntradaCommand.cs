using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Entities.Estoque;

namespace Acme.Sistemas.Services.V1.Estoque.Command.RegistrarEntrada;

public sealed record RegistrarEntradaCommand(
    Guid EstoqueId,
    Guid ProdutoId,
    decimal Quantidade,
    decimal? CustoUnitario,
    OrigemMovimento Origem,
    string? Motivo,
    Guid? FornecedorId,
    string? DocumentoReferencia,
    DateTime? DataMovimento) : IRequest<ResponseDefault<RegistrarEntradaCommandResult>>;

