using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using WebApi.DapperInfraData.Exceptions;
using WebApi.DapperInfraData.Repositories;
using WebApi.Domain.Repositories.Filters;

namespace WebApi.DapperInfraData.Services
{
    public class PaginatedSqlBuilder
    {
        private readonly WhereBuilder _where;
        private string _resultSet;
        private PaginationFilter _pagination;
        private ImmutableDictionary<string, string> _orderMap;

        public PaginatedSqlBuilder()
        {
            _where = new WhereBuilder();
        }

        public (StringBuilder Query, StringBuilder QuerySize) Build()
        {
            ValidateBuild();

            var resultSet = new StringBuilder(_resultSet);

            var where = _where.Build();

            var orderBy = new OrderByResolver(_orderMap)
                .Resolve(_pagination.OrderBy);

            var paging = SqlPagination.From(_pagination);

            resultSet
                .Append(where);

            var querySize = PaginatedCountStmt.BuildCountSql(resultSet);
            var paginatedQuery = resultSet
                .AppendLine()
                .Append(orderBy)
                .AppendLine(paging);

            return (Query: paginatedQuery, QuerySize: querySize);
        }

        public PaginatedSqlBuilder WithResultSet(string resultSet)
        {
            _resultSet = resultSet;
            return this;
        }

        public PaginatedSqlBuilder MappingOrderWith(string key, string value) =>
            MappingOrderWith(new Dictionary<string, string> { { key, value } });

        public PaginatedSqlBuilder MappingOrderWith(Dictionary<string, string> orderMap) =>
            MappingOrderWith(orderMap.ToImmutableDictionary());

        public PaginatedSqlBuilder MappingOrderWith(ImmutableDictionary<string, string> orderMap)
        {
            _orderMap = orderMap;
            return this;
        }

        public PaginatedSqlBuilder WithPagination(PaginationFilter pagination)
        {
            _pagination = pagination;
            return this;
        }

        public PaginatedSqlBuilder WithWhere(Action<WhereBuilder> where)
        {
            where(_where);
            return this;
        }

        private void ValidateBuild()
        {
            if (string.IsNullOrWhiteSpace(_resultSet))
            {
                throw new PaginatedSqlBuilderException();
            }

            if (_pagination.Equals((PaginationFilter)default))
            {
                throw new PaginatedSqlBuilderException();
            }
        }
    }
}
