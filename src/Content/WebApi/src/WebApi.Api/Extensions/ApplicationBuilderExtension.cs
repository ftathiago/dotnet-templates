using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using WebApi.Api.Filters;

namespace WebApi.Api.Extensions
{
    public static class ApplicationBuilderExtension
    {
        public static IApplicationBuilder UseCustomPathBase(
            this IApplicationBuilder app,
            string basePath) => app.Use(async (context, next) =>
        {
            context.Request.PathBase = basePath;
            await next();
        });

        public static IApplicationBuilder UseCustomCors(
            this IApplicationBuilder app) =>
            app
                .UseCors(policyConfiguration => policyConfiguration
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .WithOrigins(app.ApplicationServices
                        .GetRequiredService<IConfiguration>()
                        .GetSection("AllowedCorsOrigins")?
                        .Value ?? "*"));

        public static IApplicationBuilder UseCorrelationIdMiddleware(
            this IApplicationBuilder app) => app
            .UseMiddleware<CorrelationIdMiddleware>();

        public static IApplicationBuilder UseLocalizationConfig(
            this IApplicationBuilder app) => app
            .UseRequestLocalization(
                app.ApplicationServices
                    .GetService<IOptions<RequestLocalizationOptions>>().Value);
    }
}
