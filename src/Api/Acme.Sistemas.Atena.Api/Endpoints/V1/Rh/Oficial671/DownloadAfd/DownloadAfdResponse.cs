namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Oficial671.DownloadAfd;

public sealed record DownloadAfdResponse(string FileName, long Tamanho, string HashSha256);
