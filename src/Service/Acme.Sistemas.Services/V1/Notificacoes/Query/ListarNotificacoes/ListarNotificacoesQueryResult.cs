namespace Acme.Sistemas.Services.V1.Notificacoes.Query.ListarNotificacoes;

public sealed record ListarNotificacoesQueryResult(IReadOnlyList<NotificacaoItem> Itens);

public sealed record NotificacaoItem(
    Guid Id,
    string Tipo,
    string Titulo,
    string Mensagem,
    string? Link,
    bool Lida,
    DateTime CriadaEm);
