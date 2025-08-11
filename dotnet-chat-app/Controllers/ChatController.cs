using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly OpenAIClientService _client;
    public ChatController(OpenAIClientService client) => _client = client;

    [HttpPost]
    public async Task<ActionResult<ChatResponse>> Post([FromBody] ChatRequest req, CancellationToken ct)
    {
        if (req.messages is null || req.messages.Count == 0)
            return BadRequest("messages required");
        try
        {
            var content = await _client.ChatAsync(req.messages, ct);
            return Ok(new ChatResponse { content = content });
        }
        catch (Exception ex)
        {
            // Return detailed message to help diagnose 400/401/429/etc.
            return BadRequest(new { error = ex.Message });
        }
    }
}
