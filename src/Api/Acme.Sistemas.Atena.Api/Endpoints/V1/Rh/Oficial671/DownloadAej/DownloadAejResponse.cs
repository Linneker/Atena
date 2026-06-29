namespace Acme.Sistemas.Atena.Api.Endpoints.V1.Rh.Oficial671.DownloadAej;

public sealed record DownloadAejResponse(string FileName, long Tamanho, string HashSha256);
