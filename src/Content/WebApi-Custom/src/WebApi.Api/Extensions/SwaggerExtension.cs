using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using WebApi.Api.Configurations;

namespace WebApi.Api.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class SwaggerExtension
    {
        public static IServiceCollection ConfigSwagger(this IServiceCollection services) => services
            .AddSwaggerGen(options =>
            {
                /*********************************************************
                 Uncomment if you need a Bearer token authentication

                 s.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                {
                    Description = "Input the JWT like: Bearer {your token}",
                    Name = "Authorization",
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                });
                s.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                        },
                        Array.Empty<string>()
                    },
                 });
                 *********************************************************/
                options.ExampleFilters();
                options.OperationFilter<AddResponseHeadersFilter>();
                options.CustomSchemaIds(type => type.FullName);
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
