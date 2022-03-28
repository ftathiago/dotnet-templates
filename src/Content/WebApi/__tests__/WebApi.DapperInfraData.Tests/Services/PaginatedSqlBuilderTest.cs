using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using WebApi.DapperInfraData.Exceptions;
using WebApi.DapperInfraData.Services;
using WebApi.DapperInfraData.Tests.Fixtures.DataFixtures;
using Xunit;

namespace WebApi.DapperInfraData.Tests.Services
{
    public class PaginatedSqlBuilderTest
    {
        private const string ResultSet = "Select field from Table ";

        [Fact]
        public void Should_BuildSql_When_CallBuild()
        {
            var paginationFilter = PaginationFilterFixture.Build();
            const string Condition = "fieldname = @filter";
            var expectedSql =
                $"({ResultSet}where  ({Condition}) \n\nLIMIT {paginationFilter.PageSize} " +
                $"OFFSET {paginationFilter.PageSize * paginationFilter.PageNumber}\n, " +
                $"select count(1) from ({ResultSet}where  ({Condition}) \n) as counter_result)";

            var builder = new PaginatedSqlBuilder()
                .WithResultSet(ResultSet)
                .MappingOrderWith(new Dictionary<string, string>
                {
                    { "jsonField", "sqlField" },
                }.ToImmutableDictionary())
                .WithWhere(where => where
                    .AndWith("field_value", Condition))
                .WithPagination(paginationFilter);

            // When
            var sql = builder.Build().ToString();

            // Then
            sql.Should().Be(expectedSql);
        }

        [Fact]
        public void Should_NotAddWhere_When_DoNotCallWithWhere()
        {
            // Given
            var paginationFilter = PaginationFilterFixture.Build();
            var expectedSql =
                $"({ResultSet}\nLIMIT {paginationFilter.PageSize} " +
                $"OFFSET {paginationFilter.PageSize * paginationFilter.PageNumber}\n, " +
                $"select count(1) from ({ResultSet}) as counter_result)";

            var builder = new PaginatedSqlBuilder()
                .WithResultSet(ResultSet)
                .WithPagination(paginationFilter);

            // When
            var sql = builder.Build().ToString();

            // Then
            sql.Should().NotContain("where");
            sql.Should().Be(expectedSql);
        }

        [Fact]
        public void Should_NotAddOrderWith_When_DoNotCallMappingOrderWith()
        {
            // Given
            var paginationFilter = PaginationFilterFixture.Build();
            var expectedSql =
                $"({ResultSet}\nLIMIT {paginationFilter.PageSize} " +
                $"OFFSET {paginationFilter.PageSize * paginationFilter.PageNumber}\n, " +
                $"select count(1) from ({ResultSet}) as counter_result)";

            var builder = new PaginatedSqlBuilder()
                .WithResultSet(ResultSet)
                .WithPagination(paginationFilter);

            // When
            var sql = builder.Build().ToString();

            // Then
            sql.ToLower().Should().NotContain("order by");
            sql.Should().Be(expectedSql);
        }

        [Fact]
        public void Should_ThrowException_When_DoNotCallWithResultSet()
        {
            // Given
            var paginationFilter = PaginationFilterFixture.Build();
            var builder = new PaginatedSqlBuilder()
                .WithPagination(paginationFilter);

            // When
            Action act = () => builder.Build();

            // Then
            act.Should().ThrowExactly<PaginatedSqlBuilderException>();
        }

        [Fact]
        public void Should_ThrowException_When_DoNotCallPagination()
        {
            // Given
            var builder = new PaginatedSqlBuilder()
                .WithResultSet(ResultSet);

            // When
            Action act = () => builder.Build();

            // Then
            act.Should().ThrowExactly<PaginatedSqlBuilderException>();
        }
    }
}
