using Bogus;
using FluentAssertions;
using System;
using WebApi.DapperInfraData.Services;
using WebApi.DapperInfraData.Tests.Fixtures;
using Xunit;

namespace WebApi.DapperInfraData.Tests.Services
{
    public class SqlPaginationTest
    {
        private readonly Faker _faker = Fixture.Get();

        [Fact]
        public void Should_GeneratePostgresLimitOffset_When_InputValidData()
        {
            // Given
            var pageNumber = _faker.Random.Int(min: SqlPagination.MinPageNumber);
            var pageLimit = _faker.Random.Int(
                min: SqlPagination.MinPageSize,
                max: SqlPagination.MaxPageSize);
            var expectedStmt = $"LIMIT {pageLimit} OFFSET {pageNumber * pageLimit}";

            // When
            var generatedStmt = SqlPagination.GetPagination(pageNumber, pageLimit);

            // Then
            generatedStmt.Should().Be(expectedStmt);
        }

        [Fact]
        public void Should_ThrowInvalidArgument_When_pageNumberIsLessThanZero()
        {
            // Given
            var pageNumber = _faker.Random.Int(max: SqlPagination.MinPageNumber - 1);
            var pageLimit = _faker.Random.Int(
                min: SqlPagination.MinPageSize,
                max: SqlPagination.MaxPageSize);

            // When
            Action act = () => SqlPagination.GetPagination(pageNumber, pageLimit);

            // Then
            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void Should_ThrowInvalidArgument_When_pageSizeIsLessThanMinPageSize()
        {
            // Given
            var pageNumber = _faker.Random.Int(min: SqlPagination.MinPageNumber);
            var pageLimit = _faker.Random.Int(max: SqlPagination.MinPageSize - 1);

            // When
            Action act = () => SqlPagination.GetPagination(pageNumber, pageLimit);

            // Then
            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void Should_ThrowInvalidArgument_When_pageSizeIsGreatherThanMinPageSize()
        {
            // Given
            var pageNumber = _faker.Random.Int(min: SqlPagination.MinPageNumber);
            var pageLimit = _faker.Random.Int(min: SqlPagination.MaxPageSize + 1);

            // When
            Action act = () => SqlPagination.GetPagination(pageNumber, pageLimit);

            // Then
            act.Should().Throw<ArgumentOutOfRangeException>();
        }
    }
}
