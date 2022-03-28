using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
#if (!excludeSamples)
using WebApi.Domain.Repositories;
#endif
using WebApi.EfInfraData.Contexts;
#if (!excludeSamples)
using WebApi.EfInfraData.Repositories;
#endif
using WebApi.Shared.Data.Contexts;

namespace WebApi.EfInfraData.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class InfraDataServicesExtension
    {
        public static IServiceCollection AddEfInfraData(this IServiceCollection services) =>
            services
                .AddScoped<ISampleRepository, SampleRepository>()
                .AddDbContext<WebApiDbContext>((provider, options) =>
                {
                    var configuration = provider.GetRequiredService<IConfiguration>();
#if (!isDatabasePostgres && !isDatabaseSqlServer)
                    options.UseNpgsql(configuration.GetConnectionString("Default"));
#endif
#if (isDatabasePostgres)
                    options.UseNpgsql(configuration.GetConnectionString("Default"));
#endif
#if (isDatabaseSqlServer)
                    options.UseSqlServer(configuration.GetConnectionString("Default"));
#endif
                })
                .AddScoped<IUnitOfWork, UnitOfWork>();
    }
}
