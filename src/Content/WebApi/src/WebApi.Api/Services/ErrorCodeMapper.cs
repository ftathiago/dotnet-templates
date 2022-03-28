using System.Net;
using WebApi.Domain.Exceptions;

namespace WebApi.Api.Services
{
    public static class ErrorCodeMapper
    {
        public static HttpStatusCode Map(string errorCode) => errorCode switch
        {
            ErrorCode.InvalidData => HttpStatusCode.BadRequest,
            ErrorCode.ExpectedDataNotFound => HttpStatusCode.NotFound,
            _ => HttpStatusCode.InternalServerError,
        };
    }
}
