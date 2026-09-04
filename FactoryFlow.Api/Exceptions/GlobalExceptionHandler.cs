using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace FactoryFlow.Api.Exceptions
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            int statusCode = exception switch
            {
                KeyNotFoundException => StatusCodes.Status404NotFound,
                InvalidOperationException => StatusCodes.Status409Conflict,
                _ => StatusCodes.Status500InternalServerError
            };

            string title = statusCode == StatusCodes.Status500InternalServerError
                ? "發生未預期的錯誤"
                : exception.Message;

            httpContext.Response.StatusCode = statusCode;

            ProblemDetails problemDetails = new()
            {
                Status = statusCode,
                Title = title,
                Instance = httpContext.Request.Path
            };

            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);
            return true;
        }
    }
}
