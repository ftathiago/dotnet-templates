namespace WebApi.Api.Extensions
{
    public static class StringExtension
    {
        public static string Format(this string format, params object[] args) =>
            string.Format(format, args);

        public static string ToCamelCase(this string original)
        {
            if (string.IsNullOrEmpty(original) || char.IsLower(original[0]))
            {
                return original;
            }

            return char.ToLower(original[0]) + original[1..];
        }
    }
}
