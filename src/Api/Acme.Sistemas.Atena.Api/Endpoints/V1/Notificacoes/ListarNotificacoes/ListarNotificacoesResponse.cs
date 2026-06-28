namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Notificacoes.ListarNotificacoes;

public sealed record NotificacaoItemResponse(
    Guid Id,
    string Tipo,
    string Titulo,
    string Mensagem,
    string? Link,
    bool Lida,
    DateTime CriadaEm);
