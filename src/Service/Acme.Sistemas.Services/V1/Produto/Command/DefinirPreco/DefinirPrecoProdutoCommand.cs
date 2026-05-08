using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Produto.Command.DefinirPreco;

public sealed record DefinirPrecoProdutoCommand(
    Guid ProdutoId,
    Guid TipoValorProdutoId,
    decimal Valor,
    DateTime? VigenciaInicio = null) : IRequest<ResponseDefault<DefinirPrecoProdutoCommandResult>>;

