using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Auditoria.Query.HistoricoRegistro;

public sealed record HistoricoRegistroQuery(string Entidade, Guid EntidadeId)
    : IRequest<ResponseDefault<HistoricoRegistroQueryResult>>;
