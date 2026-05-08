namespace Acme.Sistemas.Services.V1.Auditoria.Query.ExportarLogs;

public sealed record ExportarLogsQueryResult(
    int TotalRegistros,
    string HashSha256,
    string ConteudoJson);
