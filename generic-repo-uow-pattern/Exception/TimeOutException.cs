using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace generic_repo_uow_pattern.Exception
{
    public class TimeOutException : IExceptionHandler
    {
        private readonly ILogger<TimeoutException> _logger;

        public TimeOutException(ILogger<TimeoutException> logger)
        {
            _logger = logger;
        }
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, System.Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, "An timeout occured");

            if (exception is TimeoutException)
            {

                await httpContext.Response.WriteAsJsonAsync(new ProblemDetails
                {
                    Status = (int)HttpStatusCode.RequestTimeout,
                    Type = exception.GetType().Name,
                    Title = "An timeout occured",
                    Detail = exception.Message,
                    Instance = $"{httpContext.Request.Method} {httpContext.Request.Path}"
                });

                return true;
            }
            return false;
        }
    }
}
