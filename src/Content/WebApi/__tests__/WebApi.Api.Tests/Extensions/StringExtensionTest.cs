using FluentAssertions;
using WebApi.Api.Extensions;
using WebApi.Api.Tests.Fixtures;
using Xunit;

namespace WebApi.Api.Tests.Extensions
{
    public class StringExtensionTest
    {
        private readonly string _firstString = Fixture.Get().Lorem.Word();
        private readonly string _secondString = Fixture.Get().Lorem.Word();

        [Fact]
        public void Should_SimplifyFormatCall()
        {
            // Given
            const string Format = "{0} - {1}";
            var expectedString = string.Format(Format, _firstString, _secondString);

            // When
            var formatedString = Format.Format(_firstString, _secondString);

            // Then
            formatedString.Should().Be(expectedString);
        }

        [Theory]
        [InlineData("AnyString", "anyString")]
        [InlineData("", "")]
        [InlineData("camelCaseString", "camelCaseString")]
        public void Should_ConvertAnyString_To_CamelCase(
            string originalString,
            string expectedString)
        {
            // When
            var camelCaseAnyString = originalString.ToCamelCase();

            // Then
            camelCaseAnyString.Should().Be(expectedString);
        }
    }
}
