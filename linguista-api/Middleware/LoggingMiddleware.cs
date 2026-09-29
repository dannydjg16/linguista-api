using Microsoft.Net.Http.Headers;

public class LoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<LoggingMiddleware> _logger;

    public LoggingMiddleware(RequestDelegate next, ILogger<LoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        // Log the request
        await LogRequest(context);

        // Intercept the response body
        var originalBodyStream = context.Response.Body;
        await using var responseBody = new MemoryStream();
        context.Response.Body = responseBody;

        // Continue processing the request pipeline
        await _next(context);

        // Log the response
        await LogResponse(context);

        // Copy the response back to the original stream
        await responseBody.CopyToAsync(originalBodyStream);
    }

    private async Task LogRequest(HttpContext context)
    {
        var request = context.Request;

        // Never log the bearer token
        var headers = request.Headers
            .Where(h => !h.Key.Equals(HeaderNames.Authorization, StringComparison.OrdinalIgnoreCase))
            .ToList();

        _logger.LogInformation("HTTP Request Information: ");
        _logger.LogInformation("Method: {Method}", request.Method);
        _logger.LogInformation("Path: {Path}", request.Path);
        _logger.LogInformation("Headers: {Headers}", headers);

        if (IsJson(request.ContentType))
        {
            request.EnableBuffering(); // Allows reading request body multiple times
            var requestBody = await new StreamReader(request.Body).ReadToEndAsync();
            request.Body.Position = 0; // Reset stream position for further processing

            _logger.LogInformation("Body: {Body}", requestBody);
        }
    }

    private async Task LogResponse(HttpContext context)
    {
        _logger.LogInformation("HTTP Response Information: ");
        _logger.LogInformation("Status Code: {StatusCode}", context.Response.StatusCode);

        // Skip binary bodies like TTS audio and generated images
        if (IsJson(context.Response.ContentType))
        {
            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();

            _logger.LogInformation("Body: {Body}", responseBody);
        }

        context.Response.Body.Seek(0, SeekOrigin.Begin);
    }

    private static bool IsJson(string? contentType) =>
        contentType != null && contentType.Contains("json", StringComparison.OrdinalIgnoreCase);
}