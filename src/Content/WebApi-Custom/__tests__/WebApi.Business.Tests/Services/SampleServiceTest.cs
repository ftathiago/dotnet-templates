using FluentAssertions;
using Moq;
using System;
using WebApi.Business.Entities;
using WebApi.Business.Notifications;
using WebApi.Business.Repositories;
using WebApi.Business.Services;
using WebApi.Business.Tests.Fixtures;
using WebApi.Shared.Data.Contexts;
using Xunit;

namespace WebApi.Business.Tests.Services
{
    public class SampleServiceTest
    {
        private readonly Mock<ISampleRepository> _repository;
        private readonly Mock<IUnitOfWork> _unitOfWork;
        private readonly Mock<INotification> _notifications;

        public SampleServiceTest()
        {
            _repository = new Mock<ISampleRepository>(MockBehavior.Strict);
            _unitOfWork = new Mock<IUnitOfWork>(MockBehavior.Strict);
            _notifications = new Mock<INotification>(MockBehavior.Strict);
        }

        [Fact]
        public void ShouldReturnSampleEntityWhenFoundIt()
        {
            // Given
            var expectedEntity = GetSampleEntity();
            _repository
                .Setup(s => s.GetById(expectedEntity.Id))
                .Returns(expectedEntity);
            _unitOfWork.Setup(uow => uow.BeginTransaction());
            _unitOfWork.Setup(uow => uow.Commit());
            var service = new SampleService(
                _repository.Object,
                _unitOfWork.Object,
                _notifications.Object);

            // When
            var entity = service.GetSampleBy(expectedEntity.Id);

            // Then
            entity.Should().NotBeNull();
            entity.Should().Be(expectedEntity);
        }

        [Fact]
        public void ShouldReturnNullWhenNotFoundId()
        {
            // Given
            _repository
                .Setup(s => s.GetById(It.IsAny<int>()))
                .Returns(null as SampleEntity);
            _unitOfWork.Setup(uow => uow.BeginTransaction());
            _unitOfWork.Setup(uow => uow.Commit());
            var service = new SampleService(
                _repository.Object,
                _unitOfWork.Object,
                _notifications.Object);

            // When
            var entity = service.GetSampleBy(Fixture.Get().Random.Int());

            // Then
            entity.Should().BeNull();
        }

        [Fact]
        public void ShouldNotThrowExceptions()
        {
            var exceptionThrowed = new TestException();
            const int ErrorCode = 500;
            _notifications
                .Setup(mh => mh.AddMessage(
                    ErrorCode,
                    exceptionThrowed.Message));
            _repository
                .Setup(s => s.GetById(It.IsAny<int>()))
                .Throws(exceptionThrowed);
            _unitOfWork.Setup(uow => uow.BeginTransaction());
            _unitOfWork.Setup(uow => uow.Rollback());
            var service = new SampleService(
                _repository.Object,
                _unitOfWork.Object,
                _notifications.Object);

            // When
            Action act = () => service.GetSampleBy(Fixture.Get().Random.Int());

            // Then
            act.Should().NotThrow();
        }

        [Fact]
        public void ShouldRollbackWhenAnyErrorOccursInsideRepository()
        {
            var exceptionThrowed = new TestException();
            const int ErrorCode = 500;
            _notifications
                .Setup(mh => mh.AddMessage(
                    ErrorCode,
                    exceptionThrowed.Message));
            _repository
                .Setup(s => s.GetById(It.IsAny<int>()))
                .Throws(exceptionThrowed);
            _unitOfWork.Setup(uow => uow.BeginTransaction());
            _unitOfWork.Setup(uow => uow.Rollback());
            var service = new SampleService(
                _repository.Object,
                _unitOfWork.Object,
                _notifications.Object);

            // When
            var entity = service.GetSampleBy(Fixture.Get().Random.Int());

            // Then
            entity.Should().BeNull();
        }

        private static SampleEntity GetSampleEntity() => new()
        {
            Id = Fixture.Get().Random.Int(),
            TestProperty = Fixture.Get().Lorem.Sentence(),
            Active = Fixture.Get().Random.Bool(),
        };
    }
}
