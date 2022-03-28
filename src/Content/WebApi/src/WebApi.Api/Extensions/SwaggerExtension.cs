using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using WebApi.Api.Configurations;
using WebApi.Api.Filters;
using WebApi.Api.Models.OpenApiSecurity;

namespace WebApi.Api.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class SwaggerExtension
    {
        public static IServiceCollection ConfigSwagger(this IServiceCollection services) => services
            .AddSwaggerGen(options =>
            {
                options.EnableAnnotations();
                options.DescribeAllParametersInCamelCase();
                options.CustomSchemaIds(type => type.FullName);

                OpenApiSecurityScheme securityScheme = new OpenApiBearerSecurityScheme();
                OpenApiSecurityRequirement securityRequirement = new OpenApiBearerSecurityRequirement(securityScheme);
                options.AddSecurityDefinition("Bearer", securityScheme);
                options.AddSecurityRequirement(securityRequirement);

                options.ExampleFilters();
                options.OperationFilter<AddResponseHeadersFilter>();
                options.OperationFilter<TagDescriptionsDocumentFilter>();

                options.LoadDocumentationFiles();
            })
            .AddSwaggerExamplesFromAssemblyOf<Startup>()
            .AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();

        private static void LoadDocumentationFiles(this SwaggerGenOptions options)
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                var xmlDocumentationFile = $"{assembly.GetName().Name}.xml";
                var xmlDocumentationPath = Path.Combine(AppContext.BaseDirectory, xmlDocumentationFile);
                if (File.Exists(xmlDocumentationPath))
                {
                    options.IncludeXmlComments(xmlDocumentationPath);
                }
            }
        }
    }
}
