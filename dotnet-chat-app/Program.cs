using Azure.Storage.Blobs;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

// Options
builder.Services.Configure<OpenAIOptions>(builder.Configuration.GetSection("OpenAI"));
builder.Services.Configure<StorageOptions>(builder.Configuration.GetSection("Storage"));

// Services
builder.Services.AddSingleton<OpenAIClientService>();
builder.Services.AddSingleton<AzureBlobStorageService>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Serve wwwroot
app.UseDefaultFiles();
app.UseStaticFiles();

app.MapControllers();

app.Run();

// Options and DI-visible classes
public class OpenAIOptions
{
    public string Provider { get; set; } = "azure"; // azure | openai
    public string? Endpoint { get; set; }           // e.g., https://<res>.openai.azure.com
    public string ApiKey { get; set; } = "";
    public string? Deployment { get; set; }         // Required for azure
    public string? Model { get; set; }              // Required for openai
    public string ApiVersion { get; set; } = "2024-06-01";
}

public class StorageOptions
{
    public string ConnectionString { get; set; } = "";
    public string Container { get; set; } = "uploads";
}
