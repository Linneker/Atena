using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Notificacoes.Query.ListarNotificacoes;

public sealed record ListarNotificacoesQuery() : IRequest<ResponseDefault<ListarNotificacoesQueryResult>>;
