using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;
using Microsoft.Extensions.Options;

public class AzureBlobStorageService
{
    private readonly BlobContainerClient _container;

    public AzureBlobStorageService(IOptions<StorageOptions> opt)
    {
        var o = opt.Value;
        if (string.IsNullOrWhiteSpace(o.ConnectionString))
            throw new InvalidOperationException("Storage connection string not configured.");
        if (string.IsNullOrWhiteSpace(o.Container))
            o.Container = "uploads";

        var svc = new BlobServiceClient(o.ConnectionString);
        _container = svc.GetBlobContainerClient(o.Container);
        _container.CreateIfNotExists(PublicAccessType.Blob);
    }

    public async Task<Uri> UploadAsync(IFormFile file, CancellationToken ct = default)
    {
        var name = $"{Guid.NewGuid():N}_{Path.GetFileName(file.FileName)}";
        var blob = _container.GetBlobClient(name);
        await using var s = file.OpenReadStream();
        var headers = new BlobHttpHeaders { ContentType = file.ContentType };
        await blob.UploadAsync(s, new BlobUploadOptions { HttpHeaders = headers }, ct);
        return blob.Uri;
    }
}
