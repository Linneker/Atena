namespace Acme.Sistemas.Services.V1.Rh.Oficial671.Comprovantes.Command.ExportarAfd;

public sealed record ExportarAfdCommandResult(
    Guid ExportacaoId,
    string Status,
    string? ArquivoUrl,
    string? HashSha256);
