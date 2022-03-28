using FluentAssertions;
using System.Text;
using WebApi.DapperInfraData.Repositories;
using Xunit;

namespace WebApi.DapperInfraData.Tests.Repositories
{
    public class PaginatedCountStmtTest
    {
        [Fact]
        public void Should_AddAnyStringBuilderInsideFromStmt_When_StringBuilderIsNotEmpty()
        {
            // Given
            var originalSql = new StringBuilder("select * from any_table where any_condition");

            // When
            var sqlCounter = PaginatedCountStmt.BuildCountSql(originalSql);

            // Then
            sqlCounter.ToString().Should().Be(
                "select count(1) from (select * from any_table where any_condition) as counter_result");
        }
    }
}
