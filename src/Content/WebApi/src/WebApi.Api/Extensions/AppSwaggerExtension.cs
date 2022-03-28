using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace WebApi.Api.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class AppSwaggerExtension
    {
        public static IApplicationBuilder ConfigureSwagger(
            this IApplicationBuilder app,
            IApiVersionDescriptionProvider provider,
            string pathBase) =>
            app
                .UseSwagger(c =>
                {
                    c.PreSerializeFilters.Add((swaggerDoc, httpReq) =>
                    {
                        var host = httpReq.GetForwardedHost();

                        swaggerDoc.Servers = new List<OpenApiServer>
                        {
                            new OpenApiServer
                            {
                                Url = $"https://{host}{pathBase}",
                            },
                        };
                    });
                })
                .UseSwaggerUI(o => provider.ApiVersionDescriptions
                    .ToList()
                    .ForEach(d =>
                        o.SwaggerEndpoint($"/swagger/{d.GroupName}/swagger.json", d.GroupName.ToUpper())));
    }
}
