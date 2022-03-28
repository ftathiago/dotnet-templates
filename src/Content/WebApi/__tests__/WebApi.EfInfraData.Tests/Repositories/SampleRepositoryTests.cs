using Bogus;
using FluentAssertions;
using System.Collections.Generic;
using System.Linq;
using WebApi.Domain.Entities;
using WebApi.EfInfraData.Extensions;
using WebApi.EfInfraData.Models;
using WebApi.EfInfraData.Repositories;
using WebApi.EfInfraData.Tests.Fixtures;
using Xunit;

namespace WebApi.EfInfraData.Tests.Repositories
{
    public class SampleRepositoryTests
    {
        private readonly Faker _faker = Fixture.Get();
        private readonly List<SampleTable> _data;

        public SampleRepositoryTests()
        {
            _data = Enumerable.Range(1, 4)
                .Select(_ => new SampleTable
                {
                    Id = _faker.Random.Int(min: 1),
                    Active = _faker.Random.Bool(),
                    TestProperty = _faker.Lorem.Sentence(),
                })
                .ToList();
        }

        [Fact]
        public void Should_ReturnASampleEntity_When_IdExistsInDatabase()
        {
            // Given
            SeedSampleTable();
            var expectedReturn = _data[0].AsEntity();
            var existId = expectedReturn.Id;
            using var context = DbContextFixture.BuildContext();
            var repository = BuildRepository(context);

            // When
            var entityFound = repository.GetById(existId);

            // Then
            entityFound.Should().NotBeNull();
            entityFound.Should().BeEquivalentTo(expectedReturn);
        }

        [Fact]
        public void Should_ReturnNull_When_DoestNotFoundAnEntity()
        {
            // Given
            var nonExistId = _faker.Random.Int(min: 0);
            using var context = DbContextFixture.BuildContext();
            var repository = BuildRepository(context);

            // When
            var notFoundEntity = repository.GetById(nonExistId);

            // Then
            notFoundEntity.Should().BeNull();
        }

        [Fact]
        public void Should_AddASampleEntity()
        {
            // Given
            var entity = new SampleEntity
            {
                Id = _faker.Random.Int(min: 1),
                Active = _faker.Random.Bool(),
                TestProperty = _faker.Lorem.Sentence(),
            };
            using var context = DbContextFixture.BuildContext();
            var repository = BuildRepository(context);

            // When
            repository.Add(entity);
            context.SaveChanges();

            // Then
            context.SampleTables
                .First(x => x.Id == entity.Id).Should().NotBeNull();
        }

        [Fact]
        public void Should_RemoveASampleEntity()
        {
            // Given
            var existentEntity = _data[0].AsEntity();
            SeedSampleTable();
            using var context = DbContextFixture.BuildContext();
            var repository = BuildRepository(context);

            // When
            repository.Remove(existentEntity);

            // Then
            context.SampleTables
                .FirstOrDefault(x => x.Id == existentEntity.Id)
                .Active.Should().BeFalse();
        }

        [Fact]
        public void Should_UpdateAnExistingRegister()
        {
            // Given
            SeedSampleTable();
            var storedEntity = _data[0].AsEntity();
            var updatedEntity = new SampleEntity
            {
                Id = storedEntity.Id,
                TestProperty = _faker.Lorem.Sentence(),
                Active = _faker.Random.Bool(),
            };
            using var context = DbContextFixture.BuildContext();
            var repository = BuildRepository(context);

            // When
            repository.Update(updatedEntity);

            // Then
            context.SampleTables.FirstOrDefault(x => x.Id == updatedEntity.Id)
                .Should().BeEquivalentTo(updatedEntity);
        }

        private static SampleRepository BuildRepository(DbContextFixture context) =>
            new(context);

        private void SeedSampleTable()
        {
            using var context = DbContextFixture.BuildContext();
            context.Seed(_data);
        }
    }
}
