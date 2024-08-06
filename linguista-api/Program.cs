using linguista_api.Repositories;
using linguista_api.Repositories.Interfaces;
using linguista_api.Services;
using linguista_api.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Hosting;
using Okta.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddHealthChecks();

// Add Services for DI
builder.Services.AddScoped<ICompletionsRepository, CompletionsRepository>();
builder.Services.AddScoped<ICompletionsService, CompletionsService>();
builder.Services.AddHttpClient();

//builder.Services.AddAuthentication(options =>
//{
//    options.DefaultAuthenticateScheme = OktaDefaults.ApiAuthenticationScheme;
//    options.DefaultChallengeScheme = OktaDefaults.ApiAuthenticationScheme;
//    options.DefaultSignInScheme = OktaDefaults.ApiAuthenticationScheme;
//}).AddOktaWebApi(new OktaWebApiOptions()
//{
//    OktaDomain = "https://dev-7824301.okta.com",
//    AuthorizationServerId = "default",
//    Audience = "api://default"

//});

//builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

//app.UseAuthentication();
//app.UseAuthorization();

app.MapControllers();

app.MapHealthChecks("/healthz");

app.Run();