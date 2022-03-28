using FluentAssertions;
using System;
using WebApi.DapperInfraData.Services;
using Xunit;

namespace WebApi.DapperInfraData.Tests.Services
{
    public class WhereBuilderTest
    {
        [Fact]
        public void Should_NotAddAnd_When_CallAndWithOnlyOneTime()
        {
            // Given
            var expectedWhere = "where  (condition = @condition) " + Environment.NewLine;
            var value = new { Teste = "Teste" };
            var builder = new WhereBuilder();

            // When
            var where = builder
                .AndWith(value, "condition = @condition")
                .Build();

            // Then
            where.ToString().Should().Be(expectedWhere);
        }

        [Fact]
        public void Should_NotAddAnd_When_CallAndTwice()
        {
            // Given
            var expectedWhere =
                "where  (condition = @condition) " +
                Environment.NewLine +
                " and (condition = @condition) " +
                Environment.NewLine;
            var value = new { Teste = "Teste" };
            var builder = new WhereBuilder();

            // When
            var where = builder
                .AndWith(value, "condition = @condition")
                .AndWith(value, "condition = @condition")
                .Build();

            // Then
            where.ToString().Should().Be(expectedWhere);
        }

        [Fact]
        public void Should_NotAddAnd_When_CallOrWithOnlyOneTime()
        {
            // Given
            var expectedWhere = "where  (condition = @condition) " + Environment.NewLine;
            var value = new { Teste = "Teste" };
            var builder = new WhereBuilder();

            // When
            var where = builder
                .OrWith(value, "condition = @condition")
                .Build();

            // Then
            where.ToString().Should().Be(expectedWhere);
        }

        [Fact]
        public void Should_NotAddAnd_When_CallOrTwice()
        {
            // Given
            var expectedWhere =
                "where  (condition = @condition) " +
                Environment.NewLine +
                "  or (condition = @condition) " +
                Environment.NewLine;
            var value = new { Teste = "Teste" };
            var builder = new WhereBuilder();

            // When
            var where = builder
                .OrWith(value, "condition = @condition")
                .OrWith(value, "condition = @condition")
                .Build();

            // Then
            where.ToString().Should().Be(expectedWhere);
        }

        [Fact]
        public void Should_ReturnAnEmptyStringBuilder_When_NotCallAnyCondition()
        {
            // Given
            var expectedWhere = string.Empty;
            var builder = new WhereBuilder();

            // When
            var where = builder.Build();

            // Then
            where.ToString().Should().Be(expectedWhere);
        }
    }
}
