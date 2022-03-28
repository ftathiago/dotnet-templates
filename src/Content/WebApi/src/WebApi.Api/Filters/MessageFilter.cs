using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using WebApi.Api.Extensions;
using WebApi.Domain.Notifications;

namespace WebApi.Api.Filters
{
    public class MessageFilter : IActionFilter
    {
        private const string TraceIdIdentifier = "traceId";
        private readonly INotification _notifications;
        private readonly ILogger<MessageFilter> _logger;

        public MessageFilter(INotification notifications, ILogger<MessageFilter> logger)
        {
            _notifications = notifications;
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            // Do nothing
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            if (!_notifications.Any())
            {
                return;
            }

            var errors = GetProblemDetails(context);

            LogNotifications(errors);

            context.HttpContext.Response.StatusCode =
                errors.Status.Value;

            context.Result = new ObjectResult(errors);
        }

        private ProblemDetails GetProblemDetails(ActionExecutedContext context)
        {
            var errors = _notifications.ToProblemDetail();
            var traceId = Activity.Current?.Id ?? context.HttpContext.TraceIdentifier;

            errors.Extensions.Add(TraceIdIdentifier, traceId);

            return errors;
        }

        private void LogNotifications(ProblemDetails error) =>
            _logger.LogError("Errors while processing request: {0}", error);
    }
}
