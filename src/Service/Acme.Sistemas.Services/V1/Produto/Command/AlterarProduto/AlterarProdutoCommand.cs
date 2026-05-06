using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Enums;

namespace Acme.Sistemas.Services.V1.Produto.Command.AlterarProduto;

public sealed record AlterarProdutoCommand(
    Guid Id,
    string Nome,
    string? Descricao,
    string? CodigoBarras,
    string UnidadeMedida,
    Guid? TipoProdutoId,
    Guid? FornecedorId,
    decimal? CustoMedio,
    decimal? EstoqueMinimo,
    StatusAtivo Status) : IRequest<ResponseDefault<AlterarProdutoCommandResult>>;

public sealed record AlterarProdutoCommandResult(Guid Id);
