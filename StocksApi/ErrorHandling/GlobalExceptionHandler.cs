using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace StocksApi.ErrorHandling;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        int statusCode = StatusCodes.Status500InternalServerError;

        httpContext.Response.StatusCode = statusCode;

        var problemDetails = new ProblemDetails()
        {
            Status = statusCode,
            Title = "An Unhandled exception has occured",
            Type = exception.GetType().Name,
            Detail = exception.Message
        };

        await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

        return true;
    }
}

