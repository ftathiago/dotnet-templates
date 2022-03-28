using FluentAssertions;
using WebApi.Api.Services;
using Xunit;

namespace WebApi.Api.Tests.Services
{
    public class SlugifyParameterTransformerTest
    {
        [Theory]
        [InlineData("PascalCaseString", "pascal-case-string")]
        [InlineData("camelCaseString", "camel-case-string")]
        [InlineData("noncase", "noncase")]
        [InlineData(null, null)]
        public void Should_SlugifyStrings(string originalString, string expectedSlugifyString)
        {
            // Given
            var slugger = new SlugifyParameterTransformer();

            // When
            var slugifyString = slugger.TransformOutbound(originalString);

            // Then
            slugifyString.Should().Be(expectedSlugifyString);
        }
    }
}
