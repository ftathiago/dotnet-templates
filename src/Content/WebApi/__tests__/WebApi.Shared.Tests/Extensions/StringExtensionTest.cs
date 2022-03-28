using FluentAssertions;
using WebApi.Shared.Extensions;
using Xunit;

namespace WebApi.Shared.Tests.Extensions
{
    public class StringExtensionTest
    {
        [Theory]
        [InlineData("{0}", "param1", "param1")]
        [InlineData("{0}-{1}", "param1-param2", "param1", "param2")]
        public void Should_Format(string format, string expectedString, params object[] args)
        {
            // When
            var formatedString = format.Format(args);

            // Then
            formatedString.Should().Be(expectedString);
        }

        [Theory]
        [InlineData(null, "")]
        [InlineData("", "")]
        [InlineData("ControllerName", "controller-name")]
        [InlineData("controllerName", "controller-name")]
        [InlineData("controllername", "controllername")]
        [InlineData("Controllername", "controllername")]
        [InlineData("ALargeStringWithMultiplesWords", "a-large-string-with-multiples-words")]
        public void Should_Slugify(string camelCase, string expectedSlugify)
        {
            // When
            var slugString = camelCase.ToSlugify();

            // Then
            slugString.Should().Be(expectedSlugify);
        }
    }
}
