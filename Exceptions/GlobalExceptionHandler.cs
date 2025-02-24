using Microsoft.AspNetCore.Diagnostics;
using NL_THUD.Domains.Contracts;
using System.Net;

namespace NL_THUD.Exceptions
{
    public class GlobalExceptionHandler :IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> _logger;

        public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger)
        {
            _logger = logger;
        }
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            _logger.LogError(exception, exception.Message);
            var response = new ErrorResponse
            {
                Message = exception.Message,
            };
            switch (exception)
            {
                case BadHttpRequestException:
                    response.Code = (int)HttpStatusCode.BadRequest;
                    response.Title = exception.GetType().Name;
                    break;
                default:
                    response.Code = (int)HttpStatusCode.InternalServerError;
                    response.Title = "Internal Server Error";
                    break;
            }
            httpContext.Response.StatusCode = response.Code;
            await httpContext.Response.WriteAsJsonAsync(response, cancellationToken);
            return true;
        }
    }
}
