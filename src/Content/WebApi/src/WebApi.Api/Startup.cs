using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;
using System.Diagnostics.CodeAnalysis;
using WebApi.Api.Extensions;

namespace WebApi
{
    [ExcludeFromCodeCoverage]
    public class Startup
    {
        public Startup(IConfiguration configuration) =>
            Configuration = configuration;

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services) =>
            services
                .AddApiIoc()
                .ConfigureLocalization(Configuration);

        public void Configure(
            IApplicationBuilder app,
            IWebHostEnvironment env,
            IApiVersionDescriptionProvider apiVersionDescription)
        {
            app.UseLocalizationConfig();

            var pathBase = Environment.GetEnvironmentVariable("ASPNETCORE_BASEPATH");

            if (env.IsDevelopment())
            {
                app
                    .UseDeveloperExceptionPage()
                    .ConfigureSwagger(apiVersionDescription, pathBase);
            }

            app
                .UseCorrelationIdMiddleware()
                .UseSerilogRequestLogging()
                .UseHttpsRedirection()
                .UseRouting()
                .UseHealthCheck()
                .UseAuthentication()
                .UseAuthorization()
                .UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
