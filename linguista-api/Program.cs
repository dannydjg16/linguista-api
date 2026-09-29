using linguista_api.Repositories;
using linguista_api.Repositories.Interfaces;
using linguista_api.Services;
using linguista_api.Services.Interfaces;
using Auth0.AspNetCore.Authentication.Api;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Security.Claims;
using System.Threading.RateLimiting;
using linguista_api.Globals;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();

builder.Services.AddHealthChecks();

// Add Services for DI
builder.Services.AddScoped<ICompletionsRepository, CompletionsRepository>();
builder.Services.AddScoped<ICompletionsService, CompletionsService>();
builder.Services.AddHttpClient();

// Validate Auth0-issued access tokens (issuer, audience, signature, lifetime)
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddAuth0ApiAuthentication(JwtBearerDefaults.AuthenticationScheme, builder.Configuration.GetSection("Auth0"));

builder.Services.AddAuthorization();

// Per-user rate limits on the OpenAI-backed endpoints (partitioned by the token's user id, falling back to IP)
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.AddPolicy(Constants.OpenAiRateLimitPolicy, context =>
        RateLimitPartition.GetFixedWindowLimiter(GetRateLimitPartitionKey(context), _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = 30,
            Window = TimeSpan.FromMinutes(1),
            QueueLimit = 0
        }));

    options.AddPolicy(Constants.ImageRateLimitPolicy, context =>
        RateLimitPartition.GetFixedWindowLimiter(GetRateLimitPartitionKey(context), _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = 10,
            Window = TimeSpan.FromHours(1),
            QueueLimit = 0
        }));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<LoggingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

// After auth so unauthenticated requests get 401 without using up a user's quota
app.UseRateLimiter();

app.MapControllers();

app.MapHealthChecks("/healthz");

app.Run();

static string GetRateLimitPartitionKey(HttpContext context) =>
    context.User.FindFirstValue(ClaimTypes.NameIdentifier)
    ?? context.Connection.RemoteIpAddress?.ToString()
    ?? "anonymous";