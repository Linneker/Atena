namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Oficial671.ExportarAej;

public sealed record ExportarAejResponse(
    Guid ExportacaoId, string Status, string? ArquivoUrl, string? AssinaturaUrl, string? HashSha256);
