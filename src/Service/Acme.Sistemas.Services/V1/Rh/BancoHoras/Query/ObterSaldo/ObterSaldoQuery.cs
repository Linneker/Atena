using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Rh.BancoHoras.Query.ObterSaldo;

public sealed record ObterSaldoQuery(Guid FuncionarioId, string Competencia)
    : IRequest<ResponseDefault<ObterSaldoQueryResult>>;
