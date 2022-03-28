using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using WebApi.Api.Models;
using WebApi.Api.Services;
using WebApi.Domain.Notifications;
using WebApi.Shared.Extensions;

namespace WebApi.Api.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class NotificationsExtension
    {
        public static ProblemDetails ToProblemDetail(
            this INotification notifications)
        {
            var firstErrorCode = notifications.All().First().Code;
            var error = new ProblemDetails
            {
                Type = "about:blank",
                Title = "Notification Domain",
                Detail = "An issue occurred while processing your request.",
                Status = ErrorCodeMapper.Map(firstErrorCode)
                    .AsInteger(),
            };

            error.AddErrors(notifications
                .All()
                .Select(message => new Error
                {
                    Code = message.Code,
                    Message = message.Content,
                }));

            return error;
        }
    }
}
