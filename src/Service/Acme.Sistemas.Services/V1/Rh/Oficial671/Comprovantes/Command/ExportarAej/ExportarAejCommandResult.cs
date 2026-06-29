namespace Acme.Sistemas.Services.V1.Rh.Oficial671.Comprovantes.Command.ExportarAej;

public sealed record ExportarAejCommandResult(
    Guid ExportacaoId,
    string Status,
    string? ArquivoUrl,
    string? AssinaturaUrl,
    string? HashSha256);
