using System;
using System.Linq;
using System.Text;

namespace WebApi.Shared.Extensions
{
    public static class ExceptionExtension
    {
        public static string GetAllMessage(this Exception exception, string separator, StringBuilder messages = null)
        {
            var stringBuilder = messages ?? new StringBuilder();

            stringBuilder
                .Append(exception.Message)
                .Append(separator);

            exception.InnerException?.GetAllMessage(separator, stringBuilder);

            if (exception is AggregateException agg)
            {
                agg.InnerExceptions
                    .ToList()
                    .ForEach(ex => ex.GetAllMessage(separator, stringBuilder));
            }

            return RemoveLastSeparator(stringBuilder, separator);
        }

        private static string RemoveLastSeparator(StringBuilder stringBuilder, string separator)
        {
            if (string.IsNullOrEmpty(separator) || separator.Equals(Environment.NewLine))
            {
                return stringBuilder.ToString();
            }

            var buildedString = stringBuilder.ToString();

            var endsWithSeparator = buildedString.EndsWith(separator);

            if (endsWithSeparator)
            {
                return buildedString.Remove(buildedString.Length - separator.Length);
            }

            return buildedString;
        }
    }
}
