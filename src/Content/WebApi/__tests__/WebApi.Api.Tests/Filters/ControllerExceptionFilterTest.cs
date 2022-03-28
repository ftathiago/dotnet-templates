using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using WebApi.Api.Filters;
using WebApi.Api.Models;
using WebApi.Api.Tests.Fixtures;
using Xunit;

namespace WebApi.Api.Tests.Filters
{
    public class ControllerExceptionFilterTest
    {
        private readonly Faker _faker = Fixture.Get();
        private readonly DefaultHttpContext _httpContext;
        private readonly ActionContext _actionContext;
        private readonly Mock<ILogger<ControllerExceptionFilter>> _logger;

        public ControllerExceptionFilterTest()
        {
            _httpContext = new DefaultHttpContext();
            _httpContext.Request.Method = "GET";
            _actionContext = new ActionContext(
                _httpContext,
                new RouteData(),
                Mock.Of<ActionDescriptor>());
            _logger = new Mock<ILogger<ControllerExceptionFilter>>();
        }

        [Fact]
        public void Should_MarkExceptionAsHandled_When_FilterAnException()
        {
            // Given
            var errorMessage = _faker.Lorem.Sentence();
            var context = new ExceptionContext(
                _actionContext,
                new List<IFilterMetadata>())
            {
                Exception = new Exception(errorMessage),
            };
            var filter = new ControllerExceptionFilter(_logger.Object);

            // When
            filter.OnException(context);

            // Then
            context.Result.Should().BeOfType<ObjectResult>();
            var contentResult = context.Result as ObjectResult;
            contentResult.Value.Should().BeEquivalentTo(new
            {
                Detail = "An unexpected error occurs while processing your request.",
                Status = StatusCodes.Status500InternalServerError,
                Title = "Internal server error.",
                Type = "about:blank",
            });
            var errors = (contentResult.Value as ProblemDetails)?.Extensions;
            errors.Should().NotBeNull();
            errors.Should().ContainKey("errors");
            var errorValue = errors?["errors"] as IList<Error>;
            errorValue.Should().Contain(x => x.Message.Equals(errorMessage));
            contentResult.StatusCode.Should().Be(StatusCodes.Status500InternalServerError);
        }
    }
}
