using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using WebApi.Api.Extensions;

namespace WebApi.Api.Filters
{
    public class ControllerExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<ControllerExceptionFilter> _logger;

        public ControllerExceptionFilter(ILogger<ControllerExceptionFilter> logger) =>
            _logger = logger;

        public void OnException(ExceptionContext context)
        {
            var errorMessage = context.Exception.ToErrors();

            _logger.LogError(context.Exception, "Exception {errorMessage}", errorMessage);

            context.ExceptionHandled = true;

            var error = new ProblemDetails
            {
                Type = "about:blank",
                Title = "Internal server error.",
                Detail = "An unexpected error occurs while processing your request.",
                Status = StatusCodes.Status500InternalServerError,
            };

            error.AddErrors(errorMessage);
            error.AddTraceId(context.HttpContext);

            context.Result = new ObjectResult(error)
            {
                StatusCode = error.Status,
            };
        }
    }
}
