using System;
using System.Runtime.Serialization;

namespace WebApi.DapperInfraData.Exceptions
{
    [Serializable]
    public class PaginatedSqlBuilderException : Exception
    {
        public PaginatedSqlBuilderException()
        {
        }

        public PaginatedSqlBuilderException(string message)
            : base(message)
        {
        }

        public PaginatedSqlBuilderException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected PaginatedSqlBuilderException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}
