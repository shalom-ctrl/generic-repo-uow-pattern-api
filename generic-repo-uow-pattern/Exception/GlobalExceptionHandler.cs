using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace generic_repo_uow_pattern.Exception
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, System.Exception exception, CancellationToken cancellationToken)
        {
            if (exception is TimeoutException)
            {

                await MyException.ExceptionMessage(httpContext, exception, HttpStatusCode.RequestTimeout, "A timeout occured");

                return true;
            }

            if (exception is ArgumentException)
            {

                await MyException.ExceptionMessage(httpContext, exception, HttpStatusCode.BadRequest, "A bad request occured");

                return true;
            }
            else
            {
                await MyException.ExceptionMessage(httpContext, exception, HttpStatusCode.InternalServerError, "An unexpected error occured");
                return true;
            }

            return false;
        }
    }
}
