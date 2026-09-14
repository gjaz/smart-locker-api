namespace SmartLocker.Api.Middleware;

public class CorrelationIdMiddleware
{
    private const string CorrelationHeader = "X-Correlation-ID";

    private readonly RequestDelegate _next;
    private readonly ILogger<CorrelationIdMiddleware> _logger;

    public CorrelationIdMiddleware(
        RequestDelegate next,
        ILogger<CorrelationIdMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var correlationId =
            context.Request.Headers.TryGetValue(
                CorrelationHeader,
                out var correlationHeader)
                ? correlationHeader.ToString()
                : context.TraceIdentifier;

        context.Response.Headers[CorrelationHeader] = correlationId;

        using (_logger.BeginScope(
     "CorrelationId:{CorrelationId}",
     correlationId))
        {
            await _next(context);
        }
    }
}