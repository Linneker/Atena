using Acme.Sistemas.Services.V1.Auditoria.Query.ExportarLogs;

namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Auditoria.ExportarLogs;

public static class ExportarLogsMap
{
    public static ExportarLogsQuery ToQuery(this ExportarLogsRequest request)
        => new(request.UserId, request.Entidade, request.Operacao, request.Inicio, request.Fim);

    public static ExportarLogsHashResponse ToHashResponse(this ExportarLogsQueryResult result)
        => new(result.TotalRegistros, result.HashSha256);
}
