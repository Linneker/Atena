namespace Acme.Sistemas.Infrastructure.Ged;

public interface IGedStorageProvider
{
    string Name { get; }
    Task<string> UploadAsync(string path, Stream content, string contentType, CancellationToken cancellationToken = default);
    Task<Stream> DownloadAsync(string path, CancellationToken cancellationToken = default);
    Task DeleteAsync(string path, CancellationToken cancellationToken = default);
    Task<bool> ExistsAsync(string path, CancellationToken cancellationToken = default);
}
