using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace WebApi.EfInfraData.Contexts
{
    [ExcludeFromCodeCoverage]
    public class DataContextFactory : IDesignTimeDbContextFactory<WebApiDbContext>
    {
        public WebApiDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<WebApiDbContext>();
            var configuration = SetupSource();
            Console.WriteLine(configuration.GetConnectionString("Default"));
#if (isDatabasePostgres)
            optionsBuilder.UseNpgsql(configuration.GetConnectionString("Default"));
#endif
#if (isDatabaseSqlServer)
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("Default"));
#endif
            return new WebApiDbContext(optionsBuilder.Options);
        }

        private static IConfiguration SetupSource()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            return new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{environment}.json", optional: true)
                .AddUserSecrets(Assembly.GetExecutingAssembly())
                .Build();
        }
    }
}
