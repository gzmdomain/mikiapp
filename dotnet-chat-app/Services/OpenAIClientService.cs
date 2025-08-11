using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Azure;
using Azure.AI.OpenAI;
using OpenAI.Chat;

public class OpenAIClientService
{
    private readonly OpenAIOptions _opt;

    public OpenAIClientService(Microsoft.Extensions.Options.IOptions<OpenAIOptions> opt)
    {
        _opt = opt.Value;
    }

    public async Task<string> ChatAsync(IEnumerable<AppChatMessage> messages, CancellationToken ct = default)
    {
        if (string.IsNullOrWhiteSpace(_opt.ApiKey))
            throw new InvalidOperationException("OpenAI ApiKey is not configured.");
        if (string.IsNullOrWhiteSpace(_opt.Endpoint))
            throw new InvalidOperationException("OpenAI Endpoint is not configured.");

        var endpoint = new Uri(_opt.Endpoint);
        var client = new AzureOpenAIClient(endpoint, new AzureKeyCredential(_opt.ApiKey));

        var isAiStudio = endpoint.Host.Contains("services.ai.azure.com", StringComparison.OrdinalIgnoreCase);
        var id = isAiStudio ? _opt.Model : _opt.Deployment;
        if (string.IsNullOrWhiteSpace(id))
            throw new InvalidOperationException(isAiStudio
                ? "Model is required for services.ai.azure.com endpoint."
                : "Deployment is required for classic Azure OpenAI endpoint.");

            var chatClient = client.GetChatClient(id);
            var chatMessages = new List<OpenAI.Chat.ChatMessage>();
            foreach (var m in messages)
            {
                var text = m.content ?? string.Empty;
                OpenAI.Chat.ChatMessage msg = m.role?.ToLowerInvariant() switch
                {
                    "system" => new SystemChatMessage(text),
                    "assistant" => new AssistantChatMessage(text),
                    _ => new UserChatMessage(text)
                };
                chatMessages.Add(msg);
            }

        try
        {
            var result = await chatClient.CompleteChatAsync(chatMessages, cancellationToken: ct);
            return result.Value?.Content?.FirstOrDefault()?.Text ?? string.Empty;
        }
        catch (Exception ex)
        {
            throw new InvalidOperationException($"OpenAI SDK error: {ex.Message}", ex);
        }
    }
}

public record AppChatMessage(string role, string content);

public class ChatRequest
{
    public List<AppChatMessage> messages { get; set; } = new();
}

public class ChatResponse
{
    public string content { get; set; } = string.Empty;
}

