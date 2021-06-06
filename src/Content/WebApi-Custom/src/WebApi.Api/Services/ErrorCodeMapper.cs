using System.Net;
using WebApi.Business.Notifications;

namespace WebApi.Api.Services
{
    public static class ErrorCodeMapper
    {
        public static HttpStatusCode Map(int errorCode) => errorCode switch
        {
            ErrorCode.InvalidData => HttpStatusCode.BadRequest,
            _ => HttpStatusCode.InternalServerError,
        };
    }
}
