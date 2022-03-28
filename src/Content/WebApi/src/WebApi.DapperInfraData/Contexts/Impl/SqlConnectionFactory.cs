using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace WebApi.DapperInfraData.Contexts
{
    public class SqlConnectionFactory : IDbConnectionFactory
    {
        private readonly string _config;

        public SqlConnectionFactory(IConfiguration configuration) =>
            _config = configuration.GetConnectionString("Default");

        public IDbConnection GetNewConnection() => new SqlConnection(_config);
    }
}
