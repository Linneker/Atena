using Acme.Sistemas.Services.V1.Notificacoes.Query.ListarNotificacoes;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Notificacoes.ListarNotificacoes;

public static class ListarNotificacoesMap
{
    public static ListarNotificacoesQuery ToQuery(this ListarNotificacoesRequest _)
        => new();

    public static IReadOnlyList<NotificacaoItemResponse> ToResponse(this ListarNotificacoesQueryResult result)
        => result.Itens
            .Select(i => new NotificacaoItemResponse(i.Id, i.Tipo, i.Titulo, i.Mensagem, i.Link, i.Lida, i.CriadaEm))
            .ToList();
}
