using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json.Serialization;
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
                .ConfigureAuth()
                .AddEndpoints()
                .ConfigureApiVersioning()
                .AddExternalDependencies();

        private static IServiceCollection AddEndpoints(this IServiceCollection services) =>
            services
                .AddControllers(options =>
                {
                    options.Filters.Add<ControllerExceptionFilter>();
                    options.Filters.Add<MessageFilter>();
                    options.Conventions.Add(new RouteTokenTransformerConvention(
                        new SlugifyParameterTransformer()));
                })

                .ConfigureApiBehaviorOptions(options =>
                {
                    options.SuppressMapClientErrors = true;
                    options.InvalidModelStateResponseFactory = context =>
                    {
                        var problemDetails = new ValidationProblemDetails(context.ModelState)
                        {
                            Instance = context.HttpContext.Request.Path,
                            Status = StatusCodes.Status400BadRequest,
                            Detail = "Invalid payload. Please refer to errors property for more information.",
                        };

                        return new BadRequestObjectResult(problemDetails)
                        {
                            ContentTypes = { "application/problem+json" },
                        };
                    };
                })
                .AddJsonOptions(options =>
                    options.JsonSerializerOptions.DefaultIgnoreCondition =
                        JsonIgnoreCondition.WhenWritingNull).Services
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
