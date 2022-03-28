using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Text;
using WebApi.Api.Configurations;

namespace WebApi.Api.Extensions
{
    [ExcludeFromCodeCoverage]
    internal static class ConfigureAuthentication
    {
        internal static IServiceCollection ConfigureAuth(
            this IServiceCollection services)
        {
            var jwtSettings = GetJwtSettings(services);

            return services
                .AddSingleton(jwtSettings)
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                }).AddJwtBearer(x =>
                {
                    var key = Encoding.ASCII.GetBytes(jwtSettings.Secret);
                    x.RequireHttpsMetadata = false;
                    x.SaveToken = true;
                    x.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidIssuer = jwtSettings.Issuer,
                        ValidAudience = jwtSettings.Audience,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,

                        RequireExpirationTime = true,
                    };
                }).Services;
        }

        private static JwtSettings GetJwtSettings(IServiceCollection services)
        {
            using var scope = services.BuildServiceProvider();
            return scope.GetRequiredService<IConfiguration>()?
                .GetSection("JwtSettings")
                .Get<JwtSettings>();
        }
    }
}
