using Amazon.S3;
using Amazon.S3.Model;

namespace Acme.Sistemas.Infrastructure.Ged;

public sealed class GedAwsS3StorageProvider : IGedStorageProvider
{
    public string Name => "AwsS3";

    private readonly IAmazonS3 _client;
    private readonly string _bucket;

    public GedAwsS3StorageProvider(IAmazonS3 client, string bucket)
    {
        _client = client;
        _bucket = bucket;
    }

    public async Task<string> UploadAsync(string path, Stream content, string contentType, CancellationToken cancellationToken = default)
    {
        var request = new PutObjectRequest
        {
            BucketName = _bucket,
            Key = path,
            InputStream = content,
            ContentType = contentType
        };
        await _client.PutObjectAsync(request, cancellationToken);
        return $"s3://{_bucket}/{path}";
    }

    public async Task<Stream> DownloadAsync(string path, CancellationToken cancellationToken = default)
    {
        var response = await _client.GetObjectAsync(_bucket, path, cancellationToken);
        return response.ResponseStream;
    }

    public Task DeleteAsync(string path, CancellationToken cancellationToken = default)
        => _client.DeleteObjectAsync(_bucket, path, cancellationToken);

    public async Task<bool> ExistsAsync(string path, CancellationToken cancellationToken = default)
    {
        try
        {
            await _client.GetObjectMetadataAsync(_bucket, path, cancellationToken);
            return true;
        }
        catch (AmazonS3Exception ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return false;
        }
    }
}
