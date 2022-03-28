using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Linq;
using WebApi.Api.Services;

namespace WebApi.Api.Filters
{
    public class TagDescriptionsDocumentFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var slugify = new SlugifyParameterTransformer();
            operation.Tags = operation.Tags
                .Select(tag =>
                {
                    tag.Name = slugify.TransformOutbound(tag.Name);
                    return tag;
                })
                .ToList();
        }
    }
}
