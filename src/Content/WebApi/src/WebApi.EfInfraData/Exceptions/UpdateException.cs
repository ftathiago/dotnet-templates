using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using WebApi.Shared.Extensions;

namespace WebApi.EfInfraData.Exceptions
{
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class UpdateException : InfraDataException
    {
        private const string ErrorMessage =
            "Unexpected update count received at {0} (Actual: {1}, Expected: {2})";

        public UpdateException(string tableName, int actualCount, int expectedCount)
            : base(ErrorMessage.Format(tableName, actualCount, expectedCount))
        {
        }

        protected UpdateException()
        {
        }

        protected UpdateException(string message)
            : base(message)
        {
        }

        protected UpdateException(string message, System.Exception inner)
            : base(message, inner)
        {
        }

        protected UpdateException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}
