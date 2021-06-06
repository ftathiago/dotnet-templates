using FluentAssertions;
using System;
using WebApi.Business.Notifications;
using WebApi.Business.Tests.Fixtures;
using Xunit;

namespace WebApi.Business.Tests.Notifications
{
    public class NotificationTest
    {
        [Fact]
        public void ShouldHaveTestsOnListWhenAddMessage()
        {
            // Given
            var expectedErrorCode = Fixture.Get().Random.Int();
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
        public void ShouldStringifiedMessages()
        {
            // Given
            var expectedErrorCode = Fixture.Get().Random.Int();
            var expectedMessage = Fixture.Get().Lorem.Sentence();
            INotification notification = new Notification();
            notification.AddMessage(expectedErrorCode, expectedMessage);

            // When
            var stringifiedMessage = notification.StringifyMessages();

            // Then
            stringifiedMessage.Should().BeEquivalentTo(expectedMessage);
        }

        [Fact]
        public void ShouldHaveNewLineOnStringifiedMessagesWhenHasMoreThanOneMessage()
        {
            // Given
            var expectedToken = Environment.NewLine;
            INotification notification = new Notification();
            notification.AddMessage(
                Fixture.Get().Random.Int(),
                Fixture.Get().Lorem.Sentence());
            notification.AddMessage(
                Fixture.Get().Random.Int(),
                Fixture.Get().Lorem.Sentence());

            // When
            var stringifiedMessage = notification.StringifyMessages();

            // Then
            stringifiedMessage.Should().Contain(expectedToken);
        }

        [Fact]
        public void ShouldReturnFalseWhenThereIsNotMessages()
        {
            // Given
            INotification notification = new Notification();

            // When
            var hasAny = notification.Any();

            // Then
            hasAny.Should().BeFalse();
        }

        [Fact]
        public void ShouldReturnTrueWhenThereIsNotMessages()
        {
            // Given
            INotification notification = new Notification();
            notification.AddMessage(
                Fixture.Get().Random.Int(),
                Fixture.Get().Lorem.Sentence());

            // When
            var hasAny = notification.Any();

            // Then
            hasAny.Should().BeTrue();
        }

        [Fact]
        public void ShouldAddExceptions()
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
        public void ShouldAddExceptionsWithErrorCodeSpecified()
        {
            // Given
            var expectedErrorCode = Fixture.Get().Random.Int();
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
