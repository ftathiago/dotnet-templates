using FluentAssertions;
using System;
using System.Collections.Generic;
using WebApi.DapperInfraData.Services;
using Xunit;

namespace WebApi.DapperInfraData.Tests.Services
{
    public class OrderByResolverTest
    {
        private readonly Dictionary<string, string> _fromTo = new()
        {
            { "jsonField1", "sqlField1" },
            { "jsonField2", "sqlField2" },
            { "jsonField4", "  " },
        };

        [Fact]
        public void ShouldMapFields()
        {
            // Given
            const string UserOrderBy = "jsonField1, jsonField2   desc, jsonField3 asc, jsonField4";
            var expectedOrderBy =
                "ORDER BY" +
                Environment.NewLine +
                "sqlField1 ASC, sqlField2 DESC" +
                Environment.NewLine;
            var resolver = new OrderByResolver(_fromTo);

            // When
            var resolvedOrderBy = resolver.Resolve(UserOrderBy);

            // Then
            var orderByString = resolvedOrderBy.ToString();
            orderByString.Should().Be(expectedOrderBy);
        }

        [Fact]
        public void Should_ReturnAnEmptyStringBuilder_When_OrderByIsEmpty()
        {
            // Given
            var emptyOrderBy = string.Empty;
            var expectedOrderBy = string.Empty;
            var resolver = new OrderByResolver(_fromTo);

            // When
            var resolvedOrderBy = resolver.Resolve(emptyOrderBy);

            // Then
            var orderByString = resolvedOrderBy.ToString();
            orderByString.Should().Be(expectedOrderBy);
        }

        [Fact]
        public void Should_ReturnAnEmptyStringBuilder_When_InformedFieldsDoesNotMap()
        {
            // Given
            const string NotMappedFields = "only, not, mapped, fields";
            var expectedOrderBy = string.Empty;
            var resolver = new OrderByResolver(_fromTo);

            // When
            var resolvedOrderBy = resolver.Resolve(NotMappedFields);

            // Then
            var orderByString = resolvedOrderBy.ToString();
            orderByString.Should().Be(expectedOrderBy);
        }
    }
}
