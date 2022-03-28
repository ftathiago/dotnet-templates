using System.Data;

namespace WebApi.DapperInfraData.Contexts
{
    public interface IDbConnectionFactory
    {
        IDbConnection GetNewConnection();
    }
}
