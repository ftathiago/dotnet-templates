using System.Text;

namespace WebApi.DapperInfraData.Repositories
{
    public static class PaginatedCountStmt
    {
        private const string SqlCount = "select count(1) from ({0}) as counter_result";

        public static StringBuilder BuildCountSql(StringBuilder sql) =>
            new StringBuilder()
                .AppendFormat(SqlCount, sql);
    }
}
