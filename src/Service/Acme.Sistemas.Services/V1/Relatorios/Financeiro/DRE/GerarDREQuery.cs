using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Domain.Reports;

namespace Acme.Sistemas.Services.V1.Relatorios.Financeiro.DRE;

public sealed record GerarDREQuery(DateTime Inicio, DateTime Fim) : IRequest<ResponseDefault<DREResult>>;
