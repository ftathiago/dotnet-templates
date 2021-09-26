using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Diagnostics.CodeAnalysis;
using WebApi.Api.Filters;
using WebApi.Api.Services;
using WebApi.IoC;
using WebApi.WarmUp.Extensions;

namespace WebApi.Api.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class ApiServicesExtension
    {
        public static IServiceCollection AddApiIoc(this IServiceCollection services) =>
            services
                .SlugifyRouter()
                .ConfigSwagger()
                .AddEndpoints()
                .ConfigureApiVersioning()
                .AddExternalDependencies();

        private static IServiceCollection AddEndpoints(this IServiceCollection services) =>
            services
                .AddControllers(options =>
                {
                    options.Filters.Add<ControllerExceptionFilter>();
                    options.Filters.Add<MessageFilter>();
                }).Services
                .AddHealthChecks().Services;

        private static IServiceCollection AddExternalDependencies(this IServiceCollection services) =>
            services
#if (!excludeWarmup)
                .AddWarmUp(
                    logInfo => Log.Information(logInfo),
                    logError => Log.Error(logError),
                    logTrace => Log.Verbose(logTrace))
#endif
                .ProjectsIocConfig();

        private static IServiceCollection SlugifyRouter(this IServiceCollection services) =>
            services
                .Configure<RouteOptions>(options =>
                {
                    options.LowercaseQueryStrings = true;
                    options.LowercaseUrls = true;
                    options.ConstraintMap["slugify"] = typeof(SlugifyParameterTransformer);
                });
    }
}
