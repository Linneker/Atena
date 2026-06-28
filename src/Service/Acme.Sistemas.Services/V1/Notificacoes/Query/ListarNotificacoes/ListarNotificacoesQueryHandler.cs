using Acme.Sistemas.Core.Mediators.Handler;
using Acme.Sistemas.Core.Response;

namespace Acme.Sistemas.Services.V1.Notificacoes.Query.ListarNotificacoes;

// TODO: Stub temporário. Implementação futura: tabela `notificacoes` + repositório
// + worker que enfileira (vencimentos, aprovações, estoque mínimo). Mantém
// contrato HTTP ativo para o sino do front parar de quebrar com 404.
public sealed class ListarNotificacoesQueryHandler
    : IRequestHandler<ListarNotificacoesQuery, ResponseDefault<ListarNotificacoesQueryResult>>
{
    public Task<ResponseDefault<ListarNotificacoesQueryResult>> Handle(
        ListarNotificacoesQuery request,
        CancellationToken cancellationToken)
    {
        var result = new ListarNotificacoesQueryResult(Array.Empty<NotificacaoItem>());
        return Task.FromResult(ResponseDefault<ListarNotificacoesQueryResult>.Ok(result));
    }
}
