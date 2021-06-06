using FluentAssertions;
using System.Net;
using WebApi.Api.Services;
using Xunit;

namespace WebApi.Api.Tests.Services
{
    public class ErrorCodeMapperTest
    {
        [Fact]
        public void ShouldMapInvalidDataToBadRequest()
        {
            // Given
            const int ErrorCode = Business.Notifications.ErrorCode.InvalidData;

            // When
            var httpStatus = ErrorCodeMapper.Map(ErrorCode);

            // Then
            httpStatus.Should().Be(HttpStatusCode.BadRequest);
        }
    }
}
