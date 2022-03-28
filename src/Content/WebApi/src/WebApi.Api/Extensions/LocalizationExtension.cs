using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace WebApi.Api.Extensions
{
    public static class LocalizationExtension
    {
        private const string LocalizationFolder = "Localization";

        public static IServiceCollection ConfigureLocalization(
            this IServiceCollection services,
            IConfiguration configuration) =>
            services
                .AddLocalization(setup => setup.ResourcesPath = LocalizationFolder)
                .Configure<RequestLocalizationOptions>(localizationOptions =>
                {
                    var configCultures = configuration.GetValue<string>("SupportedCultures");
                    var defaultCulture = configuration.GetValue<string>("DefaultCulture");

                    var supportedCultures = configCultures
                        .Split(',', StringSplitOptions.RemoveEmptyEntries)
                        .Select(culture => new CultureInfo(culture))
                        .ToList();

                    localizationOptions.DefaultRequestCulture = new RequestCulture(
                        culture: defaultCulture,
                        uiCulture: defaultCulture);
                    localizationOptions.SupportedCultures = supportedCultures;
                    localizationOptions.SupportedUICultures = supportedCultures;
                    localizationOptions.ApplyCurrentCultureToResponseHeaders = true;

                    localizationOptions.AddInitialRequestCultureProvider(
                        new CustomRequestCultureProvider(context =>
                        {
                            var defaultLanguage = context.Request.Headers
                                .GetDefaultAcceptLanguage(defaultCulture);

                            var isASupportedLanguage = supportedCultures
                                .Any(culture => culture.Name.Equals(defaultLanguage, StringComparison.OrdinalIgnoreCase));

                            if (!isASupportedLanguage)
                            {
                                defaultLanguage = defaultCulture;
                            }

                            return Task.FromResult(new ProviderCultureResult(defaultLanguage, defaultLanguage));
                        }));
                });
    }
}
