using Microsoft.OpenApi.Models;
using System;
using System.Diagnostics.CodeAnalysis;

namespace WebApi.Api.Models.OpenApiSecurity
{
    internal class OpenApiOAuthSecurityScheme : OpenApiSecurityScheme
    {
        [ExcludeFromCodeCoverage]
        public OpenApiOAuthSecurityScheme(string domain, string audience)
        {
            Type = SecuritySchemeType.OAuth2;
            Flows = new OpenApiOAuthFlows()
            {
                AuthorizationCode = new OpenApiOAuthFlow()
                {
                    AuthorizationUrl = new Uri($"https://{domain}/authorize"),
                    TokenUrl = new Uri($"https://{audience}/token"),
                },
            };
        }
    }
}
