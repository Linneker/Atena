using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Estoque.Query.ConsultarSaldo;

public sealed record ConsultarSaldoQuery(Guid ProdutoId, Guid? EstoqueId = null)
    : IRequest<ResponseDefault<ConsultarSaldoQueryResult>>;

