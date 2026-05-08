using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Reports;

namespace Acme.Sistemas.Services.V1.Relatorios.Financeiro.Balanco;

public sealed record GerarBalancoQuery(DateTime DataReferencia)
    : IRequest<ResponseDefault<BalancoResult>>;
