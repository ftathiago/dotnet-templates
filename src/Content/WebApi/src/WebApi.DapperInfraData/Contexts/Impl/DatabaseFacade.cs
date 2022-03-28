#nullable enable
using Dapper;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WebApi.Shared.Data.Contexts;

namespace WebApi.DapperInfraData.Contexts
{
    public class DatabaseFacade : IDatabaseFacade
    {
        private readonly IUnitOfWork _unitOfWork;

        public DatabaseFacade(IUnitOfWork unitOfWork) =>
            _unitOfWork = unitOfWork;

        public void BeginTransaction() =>
            _unitOfWork.BeginTransaction();

        public Task<IEnumerable<T>> QueryAsync<T>(StringBuilder sql, object param) =>
            _unitOfWork.Connection
                .QueryAsync<T>(sql.ToString(), param, _unitOfWork.Transaction);

        public Task<T> QuerySingleOrDefaultAsync<T>(StringBuilder sql, object param) =>
            _unitOfWork.Connection
                .QuerySingleOrDefaultAsync<T>(sql.ToString(), param, _unitOfWork.Transaction);

        public Task<T> QuerySingleOrDefaultAsync<T>(string sql, object param) =>
            _unitOfWork.Connection
                .QuerySingleOrDefaultAsync<T>(sql, param, _unitOfWork.Transaction);

        public void Rollback() =>
            _unitOfWork.Rollback();
    }
}
