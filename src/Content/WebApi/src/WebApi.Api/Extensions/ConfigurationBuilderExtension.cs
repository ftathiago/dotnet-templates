using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace WebApi.Api.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class ConfigurationBuilderExtension
    {
        public static IConfigurationBuilder SetupSource(this IConfigurationBuilder configuration)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development";
            return configuration
                .AddEnvironmentVariables()
                .AddUserSecrets(Assembly.GetExecutingAssembly())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true);
        }
    }
}
