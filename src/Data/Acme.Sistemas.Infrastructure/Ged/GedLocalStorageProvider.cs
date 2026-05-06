namespace Acme.Sistemas.Infrastructure.Ged;

public sealed class GedLocalStorageProvider : IGedStorageProvider
{
    public string Name => "Local";

    private readonly string _basePath;

    public GedLocalStorageProvider(string basePath)
    {
        _basePath = basePath;
        Directory.CreateDirectory(_basePath);
    }

    public async Task<string> UploadAsync(string path, Stream content, string contentType, CancellationToken cancellationToken = default)
    {
        var fullPath = Path.Combine(_basePath, path);
        var dir = Path.GetDirectoryName(fullPath);
        if (!string.IsNullOrEmpty(dir)) Directory.CreateDirectory(dir);

        await using var fs = File.Create(fullPath);
        await content.CopyToAsync(fs, cancellationToken);
        return fullPath;
    }

    public Task<Stream> DownloadAsync(string path, CancellationToken cancellationToken = default)
    {
        var fullPath = Path.Combine(_basePath, path);
        return Task.FromResult<Stream>(File.OpenRead(fullPath));
    }

    public Task DeleteAsync(string path, CancellationToken cancellationToken = default)
    {
        var fullPath = Path.Combine(_basePath, path);
        if (File.Exists(fullPath)) File.Delete(fullPath);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsAsync(string path, CancellationToken cancellationToken = default)
    {
        var fullPath = Path.Combine(_basePath, path);
        return Task.FromResult(File.Exists(fullPath));
    }
}
