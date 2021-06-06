using Bogus;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using Moq;
using System.Collections.Generic;
using System.Net;
using WebApi.Api.Filters;
using WebApi.Api.Tests.Fixtures;
using WebApi.Business.Notifications;
using Xunit;

namespace WebApi.Api.Tests.Filters
{
    public class MessageFilterTest
    {
        private readonly Mock<INotification> _notification;
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
            var filter = new MessageFilter(_notification.Object);

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
            var expectedOriginalData = context.Result;
            _notification
                .Setup(n => n.Any())
                .Returns(HasMessages);
            _notification
                .SetupGet(n => n.ErrorCode)
                .Returns(ErrorCode.InvalidData);
            _notification
                .Setup(n => n.StringifyMessages())
                .Returns(expectedErrorMessage);
            var filter = new MessageFilter(_notification.Object);

            // When
            filter.OnActionExecuted(context);
            var contextResult = context.Result as ObjectResult;

            // Then
            context.HttpContext.Response.StatusCode.Should().Be(ExpectedStatusCode);
            contextResult.Should().NotBeNull();
            contextResult.Value.Should().BeEquivalentTo(new
            {
                errorMessage = expectedErrorMessage,
                originalData = expectedOriginalData,
            });
        }
    }
}
