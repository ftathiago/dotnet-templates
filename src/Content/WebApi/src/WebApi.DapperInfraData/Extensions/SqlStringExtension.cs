#nullable enable
namespace WebApi.DapperInfraData.Extensions
{
    public static class SqlStringExtension
    {
        public static string? ToSqlWildcard(this string? original) =>
            string.IsNullOrWhiteSpace(original)
            ? null
            : original?.Replace("*", "%");
    }
}
