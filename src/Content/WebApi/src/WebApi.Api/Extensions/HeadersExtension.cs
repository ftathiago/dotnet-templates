using Microsoft.AspNetCore.Http;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace WebApi.Api.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class HeadersExtension
    {
        private const string CorrelationId = "X-Correlation-ID";
        private const string ForwardedHost = "X-Forwarded-Host";
        private const string AcceptLanguageHeader = "Accept-Language";

        public static string GetCorrelationId(this IHeaderDictionary headers)
        {
            var hasCorrelationId = headers.TryGetValue(CorrelationId, out var correlationId);
            if (!hasCorrelationId)
            {
                correlationId = Guid.NewGuid().ToString();
            }

            return correlationId;
        }

        public static string GetForwardedHost(this HttpRequest httpRequest)
        {
            var host = httpRequest.Headers[ForwardedHost];
            if (string.IsNullOrWhiteSpace(host))
            {
                host = httpRequest.Host.Value;
            }

            return host;
        }

        public static string GetDefaultAcceptLanguage(
            this IHeaderDictionary headers,
            string defaultLanguage = "")
        {
            var acceptHeader = headers[AcceptLanguageHeader].ToString()
                .Split(',')
                .FirstOrDefault();

            if (string.IsNullOrWhiteSpace(acceptHeader))
            {
                acceptHeader = defaultLanguage;
            }

            return acceptHeader;
        }
    }
}
