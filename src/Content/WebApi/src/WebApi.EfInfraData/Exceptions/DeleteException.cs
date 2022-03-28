using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using WebApi.Shared.Extensions;

namespace WebApi.EfInfraData.Exceptions
{
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class DeleteException : InfraDataException
    {
        private const string ErrorMessage =
            "Unexpected delete count received at {0} (Actual: {1}, Expected: {2})";

        public DeleteException(string tableName, int actualCount, int expectedCount)
            : base(ErrorMessage.Format(tableName, actualCount, expectedCount))
        {
        }

        protected DeleteException()
        {
        }

        protected DeleteException(string message)
            : base(message)
        {
        }

        protected DeleteException(string message, System.Exception inner)
            : base(message, inner)
        {
        }

        protected DeleteException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}
