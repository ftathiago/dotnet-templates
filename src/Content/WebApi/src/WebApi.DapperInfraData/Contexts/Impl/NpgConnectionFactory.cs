using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Data;

namespace WebApi.DapperInfraData.Contexts
{
    public class NpgConnectionFactory : IDbConnectionFactory
    {
        private readonly string _config;

        public NpgConnectionFactory(IConfiguration configuration) =>
            _config = configuration.GetConnectionString("Default");

        public IDbConnection GetNewConnection() => new NpgsqlConnection(_config);
    }
}
