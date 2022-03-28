using FluentAssertions;
using System.Net;
using WebApi.Api.Services;
using Xunit;

namespace WebApi.Api.Tests.Services
{
    public class ErrorCodeMapperTest
    {
        [Theory]
        [InlineData(Domain.Exceptions.ErrorCode.InvalidData, HttpStatusCode.BadRequest)]
        [InlineData(Domain.Exceptions.ErrorCode.PersistingError, HttpStatusCode.InternalServerError)]
        public void Should_MapToExpectedHttpStatus_When_ErrorCodeIsY(string errorCode, HttpStatusCode expectedHttpStatus)
        {
            // When
            var httpStatus = ErrorCodeMapper.Map(errorCode);

            // Then
            httpStatus.Should().Be(expectedHttpStatus);
        }
    }
}
