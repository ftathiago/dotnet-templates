using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using WebApi.Shared.Extensions;

namespace WebApi.Shared.Exceptions
{
    [ExcludeFromCodeCoverage]
    [Serializable]
    public class NotOpenTransactionException : UnitOfWorkException
    {
        private const string ErrorMessage =
            "There is no transaction openend to {0}";

        public NotOpenTransactionException()
        {
        }

        public NotOpenTransactionException(string operation)
            : base(ErrorMessage.Format(operation))
        {
        }

        public NotOpenTransactionException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected NotOpenTransactionException(
            SerializationInfo info,
            StreamingContext context)
            : base(info, context)
        {
        }
    }
}
