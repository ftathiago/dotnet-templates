using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using WebApi.Api.Models;

namespace WebApi.Api.Extensions
{
    [ExcludeFromCodeCoverage]
    public static class ProblemDetailsExtension
    {
        public static void AddErrors(
            this ProblemDetails problemDetails,
            IEnumerable<Error> errors) =>
            problemDetails
                .Extensions.Add(nameof(errors), errors);

        public static void AddTraceId(
            this ProblemDetails problemDetails,
            HttpContext context) => problemDetails
            .Extensions.Add(
                "traceId",
                Activity.Current?.Id ?? context.TraceIdentifier);
    }
}
