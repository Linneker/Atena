namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Oficial671.ExportarAfd;

public sealed record ExportarAfdResponse(
    Guid ExportacaoId, string Status, string? ArquivoUrl, string? HashSha256);
