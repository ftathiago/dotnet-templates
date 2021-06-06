using System.ComponentModel;

namespace WebApi.Business.Notifications
{
    public static class ErrorCode
    {
        public const int InvalidData = 1;

        public static string Description(int errorCode) => errorCode switch
        {
            InvalidData => "Invalid data.",
            _ => "Not specified error.",
        };
    }
}
