using FluentAssertions;
using System;
using WebApi.Domain.Notifications;
using WebApi.Domain.Tests.Fixtures;
using Xunit;

namespace WebApi.Domain.Tests.Notifications
{
    public class NotificationTest
    {
        [Fact]
        public void Should_HaveMessagesOnList_When_AddMessage()
        {
            // Given
            var expectedErrorCode = Fixture.Get().Lorem.Word();
            var expectedMessage = Fixture.Get().Lorem.Sentence();
            INotification notification = new Notification();
            notification.AddMessage(expectedErrorCode, expectedMessage);

            // When
            var messages = notification.All();

            // Then
            messages.Should().Contain(message =>
                message.Code == expectedErrorCode && message.Content.Equals(expectedMessage));
        }

        [Fact]
        public void Should_StringifiedMessages()
        {
            // Given
            var expectedErrorCode = Fixture.Get().Lorem.Word();
            var expectedMessage = Fixture.Get().Lorem.Sentence();
            INotification notification = new Notification();
            notification.AddMessage(expectedErrorCode, expectedMessage);

            // When
            var stringifiedMessage = notification.StringifyMessages();

            // Then
            stringifiedMessage.Should().BeEquivalentTo(expectedMessage);
        }

        [Fact]
        public void Should_HaveNewLineOnStringifiedMessages_When_HasMoreThanOneMessage()
        {
            // Given
            var expectedToken = Environment.NewLine;
            INotification notification = new Notification();
            notification.AddMessage(
                Fixture.Get().Lorem.Word(),
                Fixture.Get().Lorem.Sentence());
            notification.AddMessage(
                Fixture.Get().Lorem.Word(),
                Fixture.Get().Lorem.Sentence());

            // When
            var stringifiedMessage = notification.StringifyMessages();

            // Then
            stringifiedMessage.Should().Contain(expectedToken);
        }

        [Fact]
        public void Should_AnyCallReturnFalse_When_ThereIsNotMessages()
        {
            // Given
            INotification notification = new Notification();

            // When
            var hasAny = notification.Any();

            // Then
            hasAny.Should().BeFalse();
        }

        [Fact]
        public void Should_AnyCallReturnTrue_When_ThereIsMessages()
        {
            // Given
            INotification notification = new Notification();
            notification.AddMessage(
                Fixture.Get().Lorem.Word(),
                Fixture.Get().Lorem.Sentence());

            // When
            var hasAny = notification.Any();

            // Then
            hasAny.Should().BeTrue();
        }

        [Fact]
        public void Should_AddExceptions()
        {
            // Given
            var expectedMessage = Fixture.Get().Lorem.Sentence();
            INotification notification = new Notification();

            // When
            notification.AddException(new Exception(expectedMessage));

            // Then
            notification.Any().Should().BeTrue();
            notification.All().Should().Contain(message => message.Code == default && message.Content.Equals(expectedMessage));
        }

        [Fact]
        public void Should_AddExceptionsWithErrorCodeSpecified()
        {
            // Given
            var expectedErrorCode = Fixture.Get().Lorem.Word();
            var expectedMessage = Fixture.Get().Lorem.Sentence();
            INotification notification = new Notification();

            // When
            notification.AddException(new Exception(expectedMessage), expectedErrorCode);

            // Then
            notification.Any().Should().BeTrue();
            notification.All().Should().Contain(message => message.Code == expectedErrorCode && message.Content.Equals(expectedMessage));
        }
    }
}
