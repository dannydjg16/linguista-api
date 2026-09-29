<div align="center">

<br/>

```
██╗     ██╗███╗   ██╗ ██████╗ ██╗   ██╗██╗███████╗████████╗ █████╗
██║     ██║████╗  ██║██╔════╝ ██║   ██║██║██╔════╝╚══██╔══╝██╔══██╗
██║     ██║██╔██╗ ██║██║  ███╗██║   ██║██║███████╗   ██║   ███████║
██║     ██║██║╚██╗██║██║   ██║██║   ██║██║╚════██║   ██║   ██╔══██║
███████╗██║██║ ╚████║╚██████╔╝╚██████╔╝██║███████║   ██║   ██║  ██║
╚══════╝╚═╝╚═╝  ╚═══╝ ╚═════╝  ╚═════╝ ╚═╝╚══════╝   ╚═╝   ╚═╝  ╚═╝
```

**Master any language. Anytime, anywhere.**

📱 iOS / Swift client: [dannydjg16/Linguista](https://github.com/dannydjg16/Linguista)

[![Swift](https://img.shields.io/badge/Swift-5.9-FA7343?style=flat-square&logo=swift&logoColor=white)](https://swift.org)
[![iOS](https://img.shields.io/badge/iOS-17.0+-000000?style=flat-square&logo=apple&logoColor=white)](https://developer.apple.com/ios/)
[![Xcode](https://img.shields.io/badge/Xcode-15.0+-147EFB?style=flat-square&logo=xcode&logoColor=white)](https://developer.apple.com/xcode/)
[![License](https://img.shields.io/badge/License-MIT-brightgreen?style=flat-square)](LICENSE)
[![PRs Welcome](https://img.shields.io/badge/PRs-welcome-blueviolet?style=flat-square)](CONTRIBUTING.md)

<br/>


</div>

# linguista-api

Backend API for **Linguista**, a language-learning app. It is an ASP.NET Core (.NET 8) Web API that sits between the client and OpenAI, so the OpenAI key never ships to the client. It handles chat completions (with an optional Farsi → English transliteration pass), text-to-speech and image generation.

Every endpoint needs an Auth0 access token and has per-user rate limits.

## Tech stack

- .NET 8 / ASP.NET Core Web API (SDK version pinned in `global.json`)
- Auth0 JWT bearer authentication (`Auth0.AspNetCore.Authentication.Api`)
- OpenAI REST API (chat completions, audio speech, image generations)
- Built-in ASP.NET Core rate limiting
- Swagger / Swashbuckle (Development only)
- GitHub Actions → Azure App Service

## Project structure

```
linguista-api/
├── Controllers/     # HTTP endpoints (OpenAiController, AccountController)
├── Services/        # Business logic (CompletionsService)
├── Repositories/    # OpenAI HTTP calls (CompletionsRepository)
├── Models/          # Request/response DTOs (Completions, TTS, Image, Account)
├── Middleware/      # Request/response logging
├── Globals/         # Constants (OpenAI URLs, config keys, rate-limit policy names)
├── Program.cs       # DI, auth, rate limiting, pipeline
└── appsettings.json
```

## Getting started

### Prerequisites

- [.NET SDK 8.0.303](https://dotnet.microsoft.com/download/dotnet/8.0) or a later 8.0.3xx feature band
- An OpenAI API key
- Access to the Auth0 tenant/API in `appsettings.json`, or your own tenant

### Configuration

| Key | Where | Description |
|---|---|---|
| `OpenAiKey` | secret / env var | OpenAI API key sent as the bearer token to OpenAI |
| `Auth0:Domain` | `appsettings.json` | Auth0 tenant domain that issues access tokens |
| `Auth0:Audience` | `appsettings.json` | API identifier that tokens must be issued for (`https://api.linguista.app`) |

Never commit the OpenAI key. For local development, set it as an environment variable:

```bash
export OpenAiKey="sk-..."
```

or use .NET user secrets:

```bash
cd linguista-api
dotnet user-secrets init
dotnet user-secrets set "OpenAiKey" "sk-..."
```

In Azure, set `OpenAiKey` as an App Service application setting.

### Run locally

```bash
dotnet restore
dotnet run --project linguista-api --launch-profile https
```

- HTTPS: `https://localhost:7244`
- HTTP: `http://localhost:5071`
- Swagger UI: `https://localhost:7244/swagger` (Development environment only)
- Health check: `GET /healthz`

## Authentication

Every endpoint except `/healthz` needs an Auth0 access token issued for the configured audience:

```
Authorization: Bearer <access_token>
```

A missing or invalid token gets `401 Unauthorized`.

## API endpoints

| Method | Route | Body | Returns |
|---|---|---|---|
| `POST` | `/OpenAi/completions` | `CompletionsRequest` | OpenAI chat completion JSON |
| `POST` | `/OpenAi/completions/transliterate` | `CompletionsRequest` | Chat completion JSON; `choices[0].message.additionalContent` holds an English transliteration of the (Farsi) reply |
| `POST` | `/OpenAi/tts` | `TtsRequest` | `audio/mpeg` stream |
| `POST` | `/OpenAi/image` | `ImageGenerationRequest` | `image/png` bytes |
| `GET` | `/accountDetails/{accountId}` | — | Work in progress (see below) |
| `GET` | `/healthz` | — | Health check (no auth) |

### Request examples

**Chat completion**

```json
POST /OpenAi/completions
{
  "model": "gpt-3.5-turbo",
  "messages": [
    { "role": "system", "content": "You are a friendly Farsi tutor." },
    { "role": "user", "content": "How do I say good morning?" }
  ],
  "max_completion_tokens": 200,
  "top_p": 1
}
```

**Text-to-speech**

```json
POST /OpenAi/tts
{
  "model": "tts-1",
  "input": "صبح بخیر",
  "voice": "alloy",
  "speed": 1.0
}
```

**Image generation**

```json
POST /OpenAi/image
{
  "model": "gpt-image-1",
  "prompt": "A cozy Persian tea house, illustrated",
  "size": "1024x1024"
}
```

The request bodies go to OpenAI mostly unchanged, so any model or parameter the matching OpenAI endpoint accepts works here. The image endpoint reads `data[0].b64_json` from OpenAI's response, so use a model that returns base64 image data.

## Rate limiting

Limits are per user. The partition key is the token's user id (`sub`), and the client IP is used when there is no user id. Limits are checked after authentication, so a rejected unauthenticated request does not count against anyone's quota.

| Policy | Applies to | Limit |
|---|---|---|
| `openai` | All `/OpenAi/*` endpoints | 30 requests / minute |
| `image` | `/OpenAi/image` (in addition to `openai`) | 10 requests / hour |

A request over the limit gets `429 Too Many Requests`.

## Logging

`LoggingMiddleware` logs each request's method, path and headers, and each response's status code, at `Information` level. It leaves out the `Authorization` header. Request and response bodies are logged only when they are JSON, so audio and image responses are skipped.

## Deployment

`.github/workflows/deploy.yml`:

- **Pull requests:** restore, build and publish only. This is the CI check.
- **Pushes to `main`:** build, then deploy the published output to the Azure Web App `linguista-appservice`. The deploy job is gated by the GitHub `production` environment, so any required reviewers or protection rules on that environment apply.

The deploy job needs the `AZURE_WEBAPP_PUBLISH_PROFILE` repository/environment secret.

## Status / known gaps

- `AccountController` (`/accountDetails/{accountId}`) is scaffolding. `AccountService` and `AccountDetails` are empty, and `IAccountService` is not registered for dependency injection, so this endpoint fails until it is implemented.
- `ErrorHandlingMiddleware` and `HttpClientBuilders` (Polly retry/timeout policies) exist but are not wired into the pipeline yet.
