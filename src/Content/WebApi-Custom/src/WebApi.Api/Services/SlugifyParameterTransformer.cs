using Microsoft.AspNetCore.Routing;
using WebApi.Shared.Extensions;

namespace WebApi.Api.Services
{
    public class SlugifyParameterTransformer : IOutboundParameterTransformer
    {
        public string TransformOutbound(object value) => value?
            .ToString()
            .ToSlugify();
    }
}
