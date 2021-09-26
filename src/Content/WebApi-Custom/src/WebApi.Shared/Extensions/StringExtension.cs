using System.Text.RegularExpressions;

namespace WebApi.Shared.Extensions
{
    public static class StringExtension
    {
        public static string Format(this string format, params object[] args) =>
            string.Format(format, args);

        public static string ToSlugify(this string text) => string.IsNullOrEmpty(text)
            ? string.Empty
            : Regex
                .Replace(
                    Regex.Replace(text, "([A-Z])([A-Z])", "$1-$2"),
                    "([a-z])([A-Z])",
                    "$1-$2")
                .ToLower();
    }
}
