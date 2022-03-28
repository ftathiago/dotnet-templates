using Bogus;
using Dapper;
using FluentAssertions;
using Moq;
using Moq.Dapper;
using System;
using System.Data;
using System.Threading.Tasks;
using WebApi.DapperInfraData.Exceptions;
using WebApi.DapperInfraData.Models;
using WebApi.DapperInfraData.Repositories;
using WebApi.DapperInfraData.Tests.Fixtures;
using WebApi.Domain.Entities;
using WebApi.Shared.Data.Contexts;
using WebApi.WarmUp.Abstractions;
using Xunit;

namespace WebApi.DapperInfraData.Tests.Repositories
{
    public class SampleRepositoryTests
    {
        private readonly Faker _faker = Fixture.Get();
        private readonly Mock<IDbConnection> _connection;
        private readonly Mock<IUnitOfWork> _uow;
        private readonly Mock<IDbTransaction> _transaction;

        public SampleRepositoryTests()
        {
            _connection = new Mock<IDbConnection>();
            _uow = new Mock<IUnitOfWork>(MockBehavior.Strict);
            _uow.SetupGet(x => x.Connection).Returns(_connection.Object);
            _transaction = new Mock<IDbTransaction>();
        }

        [Fact]
        public void Should_addASampleEntity()
        {
            // Given
            var newEntity = new SampleEntity
            {
                Id = 1,
                Active = true,
                TestProperty = "asdf",
            };
            _connection
                .SetupDapper(c => c.Execute(SampleRepositoryStmt.Insert, newEntity, null, null, null))
                .Returns(1);
            var repository = new SampleRepository(_uow.Object);

            // When
            repository.Add(newEntity);

            // Then
            Mock.VerifyAll(_connection, _uow);
        }

        [Fact]
        public void Should_ReturnAnEntity_When_FoundById()
        {
            // Given
            var expectedTableReturn = BuildTable();
            var id = expectedTableReturn.Id;
            _uow.SetupGet(x => x.Transaction)
                .Returns(_transaction.Object);
            _connection
                .SetupDapper(c => c.QueryFirstOrDefault<SampleTable>(
                    SampleRepositoryStmt.GetById,
                    new { id },
                    null,
                    null,
                    null))
                .Returns(expectedTableReturn);
            var repository = new SampleRepository(_uow.Object);

            // When
            var foundEntity = repository.GetById(id);

            // Then
            foundEntity.Should().BeOfType<SampleEntity>();
            foundEntity.Should().BeEquivalentTo(new
            {
                expectedTableReturn.Id,
                expectedTableReturn.Active,
                expectedTableReturn.TestProperty,
            });
        }

        [Fact]
        public void Should_RemoveEntity()
        {
            // Given
            var model = BuildEntity();
            _connection
                .SetupDapper(x => x.Execute(
                    SampleRepositoryStmt.Remove,
                    new { model.Id },
                    _transaction.Object,
                    null,
                    null))
                .Returns(1);
            _uow
                .SetupGet(x => x.Transaction)
                .Returns(_transaction.Object);
            var repository = new SampleRepository(_uow.Object);

            // When
            repository.Remove(model);

            // Then
            Mock.VerifyAll(_uow, _connection);
        }

        [Fact]
        public void Should_ThrowDeleteException_When_RemoveMoreThanOneRegister()
        {
            // Given
            var model = BuildEntity();
            _connection
                .SetupDapper(x => x.Execute(
                    SampleRepositoryStmt.Remove,
                    new { model.Id },
                    _transaction.Object,
                    null,
                    null))
                .Returns(_faker.Random.Int(min: 2));
            _uow
                .SetupGet(x => x.Transaction)
                .Returns(_transaction.Object);
            var repository = new SampleRepository(_uow.Object);

            // When
            Action act = () => repository.Remove(model);

            // Then
            act.Should().ThrowExactly<DeleteException>();
            Mock.VerifyAll(_uow, _connection);
        }

        [Fact]
        public void Should_DoNothing_When_ModelIsAlreadyRemoved()
        {
            // Given
            var model = BuildEntity();
            _connection
                .SetupDapper(x => x.Execute(
                    SampleRepositoryStmt.Remove,
                    new { model.Id },
                    _transaction.Object,
                    null,
                    null))
                .Returns(0);
            _uow
                .SetupGet(x => x.Transaction)
                .Returns(_transaction.Object);
            var repository = new SampleRepository(_uow.Object);

            // When
            repository.Remove(model);

            // Then
            Mock.VerifyAll(_uow, _connection);
        }

        [Fact]
        public void Should_UpdateAnEntity()
        {
            // Given
            var model = BuildEntity();
            _connection
                .SetupDapper(x => x.Execute(
                    SampleRepositoryStmt.Update,
                    model,
                    null,
                    null,
                    null))
                .Returns(1);
            var repository = new SampleRepository(_uow.Object);

            // When
            repository.Update(model);

            // Then
            Mock.VerifyAll(_uow, _connection);
        }

        [Fact]
        public void Should_ThrowUpdateException_When_UpdateMoreThanOneUnit()
        {
            // Given
            var model = BuildEntity();
            _connection
                .SetupDapper(x => x.Execute(
                    SampleRepositoryStmt.Update,
                    model,
                    null,
                    null,
                    null))
                .Returns(_faker.Random.Int(min: 2));
            var repository = new SampleRepository(_uow.Object);

            // When
            Action act = () => repository.Update(model);

            // Then
            act.Should().ThrowExactly<UpdateException>();
            Mock.VerifyAll(_uow, _connection);
        }

        [Fact]
        public void Should_DoNothing_When_DoesNotUpdateAnyRegister()
        {
            // Given
            var model = BuildEntity();
            _connection
                .SetupDapper(x => x.Execute(
                    SampleRepositoryStmt.Update,
                    model,
                    null,
                    null,
                    null))
                .Returns(_faker.Random.Int(max: 0));
            var repository = new SampleRepository(_uow.Object);

            // When
            repository.Update(model);

            // Then
            Mock.VerifyAll(_uow, _connection);
        }

        [Fact]
        public void Should_CallGetByIdWithMinusOne_When_WarmupApplication()
        {
            // Given
            const int WarmupId = -1;
            var expectedTableReturn = BuildTable();

            _uow.SetupGet(x => x.Transaction)
                .Returns(_transaction.Object);
            _connection
                .SetupDapper(c => c.QueryFirstOrDefault<SampleTable>(
                    SampleRepositoryStmt.GetById,
                    new { WarmupId },
                    null,
                    null,
                    null))
                .Returns(expectedTableReturn);
            IWarmUpCommand repository = new SampleRepository(_uow.Object);

            // When
            var result = repository.Execute();

            // Then
            result.Should().Be(Task.CompletedTask);
            Mock.VerifyAll(_connection, _uow);
        }

        private static SampleTable BuildTable() => Fixture.Get<SampleTable>()
            .RuleFor(x => x.Id, (fk, _) => fk.Random.Int(min: 1))
            .RuleFor(x => x.Active, (fk, _) => fk.Random.Bool())
            .RuleFor(x => x.TestProperty, (fk, _) => fk.Lorem.Sentence())
            .Generate();

        private static SampleEntity BuildEntity() => Fixture.Get<SampleEntity>()
            .RuleFor(x => x.Id, (fk, _) => fk.Random.Int(min: 1))
            .RuleFor(x => x.Active, (fk, _) => fk.Random.Bool())
            .RuleFor(x => x.TestProperty, (fk, _) => fk.Lorem.Sentence())
            .Generate();
    }
}
