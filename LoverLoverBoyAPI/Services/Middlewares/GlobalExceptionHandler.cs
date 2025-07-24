using Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Services.Middlewares;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var exceptionResponse = new ProblemDetails
        {
            Title = exception.Source,
        };
        if (exception is BaseException ex)
        {
            httpContext.Response.StatusCode = (int)ex.StatusCode;
            exceptionResponse.Detail = ex.Message;
        }
        else
        {
            exceptionResponse.Detail = exception.Message;
        }

        await httpContext.Response.WriteAsJsonAsync(exceptionResponse, cancellationToken).ConfigureAwait(false);
        return true;
    }
}