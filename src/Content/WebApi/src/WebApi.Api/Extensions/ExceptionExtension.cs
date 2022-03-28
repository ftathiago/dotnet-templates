using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using WebApi.Api.Models;

namespace WebApi.Api.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class ExceptionExtension
    {
        public static IList<Error> ToErrors(
            this Exception exception,
            IList<Error> errors = null)
        {
            var exceptionError = errors ?? new List<Error>();

            exceptionError.Add(new Error
            {
                Message = exception.Message,
            });

            exception.InnerException?.ToErrors(exceptionError);

            if (exception is AggregateException agg)
            {
                foreach (var ex in agg.InnerExceptions)
                {
                    ex.ToErrors(exceptionError);
                }
            }

            return exceptionError;
        }
    }
}
