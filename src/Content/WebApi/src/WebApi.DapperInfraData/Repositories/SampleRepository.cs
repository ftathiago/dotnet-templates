using Dapper;
using System.Threading.Tasks;
using WebApi.DapperInfraData.Exceptions;
using WebApi.DapperInfraData.Extensions;
using WebApi.DapperInfraData.Models;
using WebApi.Domain.Entities;
using WebApi.Domain.Repositories;
using WebApi.Shared.Data.Contexts;
using WebApi.WarmUp.Abstractions;

namespace WebApi.DapperInfraData.Repositories
{
    public class SampleRepository : IWarmUpCommand, ISampleRepository
    {
        private readonly IUnitOfWork _unitOfWork;

        public SampleRepository(IUnitOfWork unitOfWork) =>
            _unitOfWork = unitOfWork;

        public void Add(SampleEntity model) => _unitOfWork.Connection
            .Execute(SampleRepositoryStmt.Insert, model);

        public SampleEntity GetById(int id) =>
            _unitOfWork.Connection
                .QueryFirstOrDefault<SampleTable>(
                    SampleRepositoryStmt.GetById,
                    new { id },
                    _unitOfWork.Transaction)?
                .AsEntity();

        public void Remove(SampleEntity model)
        {
            var alteredCount = _unitOfWork.Connection
                .Execute(
                    SampleRepositoryStmt.Remove,
                    new { model.Id },
                    _unitOfWork.Transaction);

            if (alteredCount > 1)
            {
                throw new DeleteException("Sample Table", alteredCount, 0);
            }
        }

        public void Update(SampleEntity model)
        {
            var alteredCount = _unitOfWork.Connection
                .Execute(SampleRepositoryStmt.Update, model);

            if (alteredCount > 1)
            {
                throw new UpdateException("Sample Table", alteredCount, 0);
            }
        }

        Task IWarmUpCommand.Execute()
        {
            GetById(-1);
            return Task.CompletedTask;
        }
    }
}
