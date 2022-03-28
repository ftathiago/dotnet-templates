using Microsoft.OpenApi.Models;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace WebApi.Api.Models.OpenApiSecurity
{
    internal class OpenApiOAuthSecurityRequirement : OpenApiSecurityRequirement
    {
        [ExcludeFromCodeCoverage]
        public OpenApiOAuthSecurityRequirement()
        {
            Add(
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = "oauth2",
                    },
                    Scheme = "oauth2",
                    Name = "oauth2",
                    In = ParameterLocation.Header,
                },
                new List<string>());
        }
    }
}
