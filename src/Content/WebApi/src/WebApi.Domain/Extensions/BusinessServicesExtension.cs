using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;
#if (!excludeSamples)
using WebApi.Domain.Services;
#endif

namespace WebApi.Domain.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class BusinessServicesExtension
    {
        public static IServiceCollection AddBusiness(this IServiceCollection services) =>
#if (!excludeSamples)
            services
                .AddScoped<ISampleService, SampleService>();
#endif
#if (excludeSamples)
            services;
#endif
    }
}
