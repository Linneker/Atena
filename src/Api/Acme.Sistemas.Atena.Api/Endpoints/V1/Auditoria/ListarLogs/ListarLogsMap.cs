using Acme.Sistemas.Services.V1.Auditoria.Query.ListarLogs;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Auditoria.ListarLogs;

public static class ListarLogsMap
{
    public static ListarLogsQuery ToQuery(this ListarLogsRequest request)
        => new(
            request.UserId,
            request.Entidade,
            request.Operacao,
            request.Inicio,
            request.Fim,
            request.Skip,
            request.Take);

    public static ListarLogsResponse ToResponse(this ListarLogsQueryResult result)
        => new(result.Items.Select(i => i.ToResponseItem()).ToArray(), result.Total);

    private static ListarLogsResponseItem ToResponseItem(this ListarLogsQueryItem item)
        => new(item.Id, item.UserId, item.Entidade, item.EntidadeId, item.Operacao, item.CommandTipo, item.OcorridoEm);
}
