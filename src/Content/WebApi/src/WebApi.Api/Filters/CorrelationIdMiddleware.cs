using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using WebApi.Api.Extensions;

namespace WebApi.Api.Filters
{
    [ExcludeFromCodeCoverage]
    public class CorrelationIdMiddleware
    {
        private readonly RequestDelegate _next;

        public CorrelationIdMiddleware(RequestDelegate next) =>
            _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            var correlationId = context.Request.Headers.GetCorrelationId();

            context.Items["CorrelationId"] = correlationId;

            var logger = context.RequestServices
                .GetRequiredService<ILogger<CorrelationIdMiddleware>>();
            using (logger.BeginScope("{@CorrelationId}", correlationId))
            {
                await _next(context);
            }
        }
    }
}
