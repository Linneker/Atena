using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Orcamento.Command.CriarOrcamento;

public sealed record OrcamentoItemDto(Guid ProdutoId, decimal Quantidade, decimal PrecoUnitario);

