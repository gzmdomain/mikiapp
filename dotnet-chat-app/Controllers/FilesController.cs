using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/files")]
public class FilesController : ControllerBase
{
    private readonly AzureBlobStorageService _storage;
    public FilesController(AzureBlobStorageService storage) => _storage = storage;

    [HttpPost("upload")]
    [RequestSizeLimit(20_000_000)] // ~20MB
    public async Task<IActionResult> Upload([FromForm] IFormFile file, CancellationToken ct)
    {
        if (file is null || file.Length == 0) return BadRequest("file required");
        if (string.IsNullOrWhiteSpace(file.ContentType) || !file.ContentType.StartsWith("image/"))
            return BadRequest("only image files are allowed");
        var uri = await _storage.UploadAsync(file, ct);
        return Ok(new { url = uri.ToString() });
    }
}
