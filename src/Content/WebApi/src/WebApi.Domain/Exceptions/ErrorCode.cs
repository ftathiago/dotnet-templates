namespace WebApi.Domain.Exceptions
{
    public static class ErrorCode
    {
        public const string InvalidData = "WebApi-1";
        public const string PersistingError = "WebApi-2";
        public const string ExpectedDataNotFound = "WebApi-3";

        public static string Description(string errorCode) => errorCode switch
        {
            InvalidData => "Invalid data.",
            PersistingError => "Error persisting data.",
            ExpectedDataNotFound => "Some expected data was not found.",
            _ => "Not specified error.",
        };
    }
}
