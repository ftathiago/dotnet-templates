using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using WebApi.Api.Services;
using WebApi.Business.Notifications;

namespace WebApi.Api.Filters
{
    public class MessageFilter : IActionFilter
    {
        private readonly INotification _notifications;

        public MessageFilter(INotification notifications) =>
            _notifications = notifications;

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

            context.HttpContext.Response.StatusCode =
                MappingErrorToHttpStatusCode(_notifications.ErrorCode);
            context.Result = new ObjectResult(new
            {
                errorMessage = _notifications.StringifyMessages(),
                originalData = context.Result,
            });
        }

        public int MappingErrorToHttpStatusCode(int errorCode) =>
            (int)ErrorCodeMapper.Map(errorCode);
    }
}
