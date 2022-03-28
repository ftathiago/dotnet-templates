#nullable enable
using System.Text;

namespace WebApi.DapperInfraData.Services
{
    public class WhereBuilder
    {
        private readonly StringBuilder _where;

        public WhereBuilder() =>
            _where = new StringBuilder();

        public StringBuilder Build()
        {
            if (_where.Length == 0)
            {
                return new StringBuilder();
            }

            return new StringBuilder("where ")
                .Append(_where.ToString()[4..]);
        }

        public WhereBuilder AndWith(object? paramValue, string condition)
        {
            if (paramValue is not null)
            {
                _where
                    .Append(" and (")
                    .Append(condition)
                    .AppendLine(") ");
            }

            return this;
        }

        public WhereBuilder OrWith(object? paramValue, string condition)
        {
            if (paramValue is not null)
            {
                _where
                    .Append("  or (")
                    .Append(condition)
                    .AppendLine(") ");
            }

            return this;
        }
    }
}
