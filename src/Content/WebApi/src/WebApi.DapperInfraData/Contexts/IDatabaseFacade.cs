#nullable enable
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WebApi.DapperInfraData.Contexts
{
    public interface IDatabaseFacade
    {
        void BeginTransaction();

        void Rollback();

        Task<IEnumerable<T>> QueryAsync<T>(
            StringBuilder sql,
            object param);

        Task<T> QuerySingleOrDefaultAsync<T>(
            StringBuilder sql,
            object param);

        Task<T> QuerySingleOrDefaultAsync<T>(
            string sql,
            object param);
    }
}
