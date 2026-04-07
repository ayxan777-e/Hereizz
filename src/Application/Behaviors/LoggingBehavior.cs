using System.Diagnostics;
using System.Text.Json;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace Application.Behaviors;

public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = false
    };

    private readonly ILogger<LoggingBehavior<TRequest, TResponse>> _logger;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LoggingBehavior(
        ILogger<LoggingBehavior<TRequest, TResponse>> logger,
        IHttpContextAccessor httpContextAccessor)
    {
        _logger = logger;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        var requestName = typeof(TRequest).Name;
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.NameIdentifier) ?? "anonymous";
        var stopwatch = Stopwatch.StartNew();

        _logger.LogInformation(
            "MediatR request started. Request={RequestName}, UserId={UserId}, Payload={Payload}",
            requestName,
            userId,
            SafeSerialize(request));

        try
        {
            var response = await next();

            stopwatch.Stop();
            _logger.LogInformation(
                "MediatR request completed. Request={RequestName}, UserId={UserId}, DurationMs={DurationMs}",
                requestName,
                userId,
                stopwatch.ElapsedMilliseconds);

            return response;
        }
        catch (Exception ex)
        {
            stopwatch.Stop();
            _logger.LogError(
                ex,
                "MediatR request failed. Request={RequestName}, UserId={UserId}, DurationMs={DurationMs}",
                requestName,
                userId,
                stopwatch.ElapsedMilliseconds);

            throw;
        }
    }

    private static string SafeSerialize(TRequest request)
    {
        try
        {
            return JsonSerializer.Serialize(request, JsonOptions);
        }
        catch
        {
            return "<unserializable-payload>";
        }
    }
}
