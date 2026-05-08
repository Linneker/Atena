using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Relatorios.PosicaoEstoque;

public sealed record PosicaoEstoqueQuery(Guid? EstoqueId = null)
    : IRequest<ResponseDefault<PosicaoEstoqueQueryResult>>;

