using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Produto.Command.DefinirPreco;

public sealed record DefinirPrecoProdutoCommandResult(Guid PrecoId, decimal Valor, DateTime VigenciaInicio);
