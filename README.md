# mcp_server workspace

This workspace contains a .NET 8 web app `dotnet-chat-app` with:
- Chat via Azure OpenAI (supports services.ai.azure.com model or classic *.openai.azure.com deployment)
- Image upload to Azure Blob Storage

## Local run
1. Update `dotnet-chat-app/appsettings.json` for local dev, or copy `appsettings.example.json` to `appsettings.json` and fill values.
2. Build and run:

```pwsh
cd c:/project/mcp_server/dotnet-chat-app
 dotnet build
 dotnet run
```

Visit http://localhost:5000

## Deploy to Azure App Service (global)
- Create a Web App and set App Settings keys matching `OpenAI:*` and `Storage:*`.
- Publish using your preferred method (zip deploy, az webapp up, or GitHub Actions).

## Publish to GitHub
1. Ensure secrets are not committed (we ignore `**/appsettings.json`). Commit `appsettings.example.json` instead.
2. Initialize git and make first commit:

```pwsh
cd c:/project/mcp_server
 git init
 git add .
 git commit -m "Initial commit: chat app with Azure OpenAI and Blob upload"
```

3. Create a new GitHub repo at https://github.com/new and copy its remote URL.
4. Add the remote and push:

```pwsh
git branch -M main
 git remote add origin https://github.com/<your-user>/<your-repo>.git
 git push -u origin main
```

## Optional: GitHub Actions CI
A workflow file can build the .NET app on push. Add repository secrets for any deployment steps you enable.# mcp_server / dotnet-chat-app

A .NET 8 web API + static UI for chat with Azure OpenAI and image uploads to Azure Blob Storage.

## What’s here
- `dotnet-chat-app/` — ASP.NET Core app with controllers and a simple front-end.
- `wwwroot/` — Chat UI and styles.
- `Services/` — OpenAI and Blob services.

## Local run
1. Install .NET 8 SDK.
2. Copy `dotnet-chat-app/appsettings.example.json` to `dotnet-chat-app/appsettings.json` and set:
   - `OpenAI.Endpoint`, `ApiKey`, and either `Model` (services.ai.azure.com) or `Deployment` (classic openai.azure.com).
   - `Storage.ConnectionString` and `Container`.
3. Run:
   ```powershell
   pwsh
   cd ./dotnet-chat-app
   dotnet restore
   dotnet run
   ```
4. Open http://localhost:5000

## Deploy to Azure App Service (global)
- Create Web App (Windows/Linux) and configure App Settings for OpenAI and Storage (don’t commit secrets).
- Zip deploy or `dotnet publish` + deploy via your preferred method.

## Publish to GitHub
1. Initialize the repo and commit:
   ```powershell
   pwsh
   cd c:/project/mcp_server
   git init
   git add .
   git commit -m "Initial commit: .NET chat app with Azure OpenAI + Blob uploads"
   ```
2. Create a GitHub repo (via UI or CLI), then add the remote and push:
   ```powershell
   git remote add origin https://github.com/<your-username>/<repo-name>.git
   git branch -M main
   git push -u origin main
   ```

## CI: Build on push
A simple GitHub Actions workflow is included in `.github/workflows/dotnet.yml` that restores, builds, and tests the .NET project on every push/PR.

Tip: Keep `appsettings.json` out of Git (already ignored) and commit only `appsettings.example.json`.

## Security
- Do not commit real secrets. Use Azure App Service settings or GitHub Secrets.
- Consider rotating any keys previously committed.
