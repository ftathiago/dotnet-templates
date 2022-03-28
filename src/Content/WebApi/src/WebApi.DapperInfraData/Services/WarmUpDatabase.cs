using Dapper;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using WebApi.Shared.Data.Contexts;
using WebApi.WarmUp.Abstractions;

namespace WebApi.DapperInfraData.Services
{
    [ExcludeFromCodeCoverage]
    public class WarmUpDatabase : IWarmUpCommand
    {
        private readonly IUnitOfWork _unitOfWork;

        public WarmUpDatabase(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task Execute()
        {
            await _unitOfWork.Connection.QueryAsync<int>("select 1");
        }
    }
}
