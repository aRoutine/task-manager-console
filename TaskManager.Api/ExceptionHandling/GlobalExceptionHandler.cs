using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace TaskManager.Api.ExceptionHandling;
public class GlobalExceptionHandler : IExceptionHandler
{
    private readonly ILogger _logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
    {
        _logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        _logger.LogError(
            exception,
            "Необработанная ошибка при выполнении запроса {Method} {Path}",
            httpContext.Request.Method,
            httpContext.Request.Path
        );

        ProblemDetails problemDetails = new ProblemDetails
        {
          Status = StatusCodes.Status500InternalServerError,
          Title = "Внутренняя ошибка сервера",
          Detail = "При выполнении запроса произошла непредвиденная ошибка",
          Instance = httpContext.Request.Path
        };

        problemDetails.Extensions["traceId"] = httpContext.TraceIdentifier;

        httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}