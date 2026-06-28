namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Auditoria.ExportarLogs;

// Endpoint /exportar retorna application/json (file download), sem corpo tipado.
// Endpoint complementar /exportar/hash retorna este DTO de verificação.
public sealed record ExportarLogsHashResponse(
    int TotalRegistros,
    string HashSha256);
