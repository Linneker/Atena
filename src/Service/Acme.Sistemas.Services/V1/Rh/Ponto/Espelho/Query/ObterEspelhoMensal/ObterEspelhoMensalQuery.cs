using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;
using Acme.Sistemas.Services.V1.Rh.Ponto.Engine;

namespace Acme.Sistemas.Services.V1.Rh.Ponto.Espelho.Query.ObterEspelhoMensal;

/// <summary>Competência YYYY-MM. funcionarioId obrigatório (RH consulta de qualquer; funcionário só o próprio via outro endpoint).</summary>
public sealed record ObterEspelhoMensalQuery(Guid FuncionarioId, string Competencia)
    : IRequest<ResponseDefault<GeradorEspelhoMensal.EspelhoMensal>>;
