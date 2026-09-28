using linguista_api.Repositories;
using linguista_api.Repositories.Interfaces;
using linguista_api.Services;
using linguista_api.Services.Interfaces;
using Auth0.AspNetCore.Authentication.Api;
using Microsoft.AspNetCore.Authentication.JwtBearer;

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

app.MapControllers();

app.MapHealthChecks("/healthz");

app.Run();