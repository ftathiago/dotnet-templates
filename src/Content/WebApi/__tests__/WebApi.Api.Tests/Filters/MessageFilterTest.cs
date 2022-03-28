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
using System.Linq;
using System.Net;
using WebApi.Api.Filters;
using WebApi.Api.Models;
using WebApi.Api.Tests.Fixtures;
using WebApi.Domain.Exceptions;
using WebApi.Domain.Notifications;
using Xunit;

namespace WebApi.Api.Tests.Filters
{
    public class MessageFilterTest : IDisposable
    {
        private readonly Mock<INotification> _notification;
        private readonly Mock<ILogger<MessageFilter>> _logger;
        private readonly DefaultHttpContext _httpContext;
        private readonly ActionContext _actionContext;
        private readonly Faker _faker = Fixture.Get();

        public MessageFilterTest()
        {
            _notification = new Mock<INotification>(MockBehavior.Strict);
            _httpContext = new DefaultHttpContext();
            _httpContext.Request.Method = "GET";
            _actionContext = new ActionContext(
                _httpContext,
                new RouteData(),
                Mock.Of<ActionDescriptor>());
            _logger = new Mock<ILogger<MessageFilter>>();
        }

        public void Dispose()
        {
            Mock.VerifyAll(_notification);
        }

        [Fact]
        public void ShouldDoNothingWhenNotificationIsEmpty()
        {
            // Given
            const bool HasMessages = true;
            var context = new ActionExecutedContext(
                _actionContext,
                filters: new List<IFilterMetadata>(),
                controller: new object());
            _notification
                .Setup(n => n.Any())
                .Returns(!HasMessages);
            var filter = new MessageFilter(_notification.Object, _logger.Object);

            // When
            filter.OnActionExecuted(context);

            // Then
            _notification.VerifyAll();
        }

        [Fact]
        public void ShouldChangeHttpStatusCodeWhenNotificationHasMessage()
        {
            // Given
            const bool HasMessages = true;
            const int ExpectedStatusCode = (int)HttpStatusCode.BadRequest;
            var expectedErrorMessage = _faker.Lorem.Sentence();
            var context = new ActionExecutedContext(
                _actionContext,
                filters: new List<IFilterMetadata>(),
                controller: new object());
            var expectedErrors = Enumerable.Range(1, 2)
                .Select(_ => (ErrorCode.InvalidData, expectedErrorMessage))
                .ToList();
            _notification
                .Setup(n => n.Any())
                .Returns(HasMessages);
            _notification
                .Setup(n => n.All())
                .Returns(expectedErrors);
            var filter = new MessageFilter(_notification.Object, _logger.Object);

            // When
            filter.OnActionExecuted(context);
            var contextResult = context.Result as ObjectResult;

            // Then
            context.HttpContext.Response.StatusCode.Should().Be(ExpectedStatusCode);
            contextResult.Should().NotBeNull();
            contextResult.Value.Should().BeEquivalentTo(new
            {
                Title = "Notification Domain",
                Type = "about:blank",
                Status = ExpectedStatusCode,
                Detail = "An issue occurred while processing your request.",
            });
            var errorsDict = (contextResult.Value as ProblemDetails)?.Extensions;
            errorsDict.Should().ContainKey("errors");
            (errorsDict?["errors"] as IEnumerable<Error>)?
                .Should().Contain(x => x.Message.Equals(expectedErrorMessage));
        }
    }
}
