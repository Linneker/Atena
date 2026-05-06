using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace Acme.Sistemas.Infrastructure.Ged;

public sealed class GedAzureBlobStorageProvider : IGedStorageProvider
{
    public string Name => "AzureBlob";

    private readonly BlobContainerClient _container;

    public GedAzureBlobStorageProvider(BlobContainerClient container)
    {
        _container = container;
    }

    public async Task<string> UploadAsync(string path, Stream content, string contentType, CancellationToken cancellationToken = default)
    {
        var blob = _container.GetBlobClient(path);
        await blob.UploadAsync(content, new BlobHttpHeaders { ContentType = contentType }, cancellationToken: cancellationToken);
        return blob.Uri.ToString();
    }

    public async Task<Stream> DownloadAsync(string path, CancellationToken cancellationToken = default)
    {
        var blob = _container.GetBlobClient(path);
        var response = await blob.DownloadStreamingAsync(cancellationToken: cancellationToken);
        return response.Value.Content;
    }

    public async Task DeleteAsync(string path, CancellationToken cancellationToken = default)
    {
        var blob = _container.GetBlobClient(path);
        await blob.DeleteIfExistsAsync(cancellationToken: cancellationToken);
    }

    public async Task<bool> ExistsAsync(string path, CancellationToken cancellationToken = default)
    {
        var blob = _container.GetBlobClient(path);
        return await blob.ExistsAsync(cancellationToken);
    }
}
