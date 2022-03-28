using FluentMigrator.Runner;
using FluentMigrator.Runner.Conventions;
using FluentMigrator.Runner.Initialization;
#if (isDatabaseSqlServer)
using Microsoft.Data.SqlClient;
#endif
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
#if(!isDatabaseSqlServer && !isDatabasePostgres)
using Npgsql;
#endif
#if (isDatabasePostgres)
using Npgsql;
#endif
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using WebApi.DapperInfraData.Contexts;
using WebApi.DapperInfraData.Repositories;
using WebApi.DapperInfraData.Services;
using WebApi.Domain.Repositories;
using WebApi.Shared.Data.Contexts;

namespace WebApi.DapperInfraData.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class InfraDataServicesExtension
    {
        private const string ConnectionString = "Default";

        public static IServiceCollection AddDapperInfraData(this IServiceCollection services) =>
            services
                .AddRepositories()
                .ConfigureFluentMigrator()
#if(!isDatabaseSqlServer && !isDatabasePostgres)
                .AddScoped<IDbConnectionFactory, NpgConnectionFactory>()
#endif
#if (isDatabaseSqlServer)
                .AddScoped<IDbConnectionFactory, SqlConnectionFactory>() 
#endif
#if (isDatabasePostgres)
                .AddScoped<IDbConnectionFactory, NpgConnectionFactory>()
#endif
                .AddScoped<IUnitOfWork, UnitOfWork>();

        private static IServiceCollection AddRepositories(this IServiceCollection services) =>
            services
                .AddScoped<ISampleRepository, SampleRepository>();

        private static IServiceCollection ConfigureFluentMigrator(this IServiceCollection services) =>
            services
                .AddTransient<MigrationExecutor>()
                .AddFluentMigratorCore()
                .AddSingleton<IConventionSet>(provider =>
                {
                    var connectionString = provider.GetRequiredService<IConfiguration>()
                        .GetConnectionString(ConnectionString);
#if (!isDatabasePostgres && !isDatabaseSqlServer)
                    var defaultSchema = new NpgsqlConnectionStringBuilder(connectionString)
                        .SearchPath;
#endif
#if (isDatabasePostgres)
                    var defaultSchema = new NpgsqlConnectionStringBuilder(connectionString)
                        .SearchPath;
#endif
#if (isDatabaseSqlServer)
                    var defaultSchema = new SqlConnectionStringBuilder(connectionString)
                        .InitialCatalog;                    
#endif

                    if (string.IsNullOrWhiteSpace(defaultSchema))
                    {
                        defaultSchema = "public";
                    }

                    return new DefaultConventionSet(
                        defaultSchema,
                        workingDirectory: "/");
                })
                .ConfigureRunner(rb => rb
#if (!isDatabasePostgres && !isDatabaseSqlServer)
                    .AddPostgres11_0()
#endif
#if (isDatabasePostgres)
                    .AddPostgres11_0()
#endif
#if (isDatabaseSqlServer)
                    .AddSqlServer2016()
#endif
                    .WithGlobalConnectionString(provider =>
                        provider.GetRequiredService<IConfiguration>()
                            .GetConnectionString(ConnectionString))
                    .ScanIn(Assembly.GetExecutingAssembly())
                        .For.Migrations())
                .Configure<RunnerOptions>(opt =>
                    opt.TransactionPerSession = true)
                .AddLogging(lb => lb.AddFluentMigratorConsole());
    }
}
