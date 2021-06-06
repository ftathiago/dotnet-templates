using System;
using System.Net;
using WebApi.Business.Entities;
using WebApi.Business.Notifications;
using WebApi.Business.Repositories;
using WebApi.Shared.Data.Contexts;

namespace WebApi.Business.Services
{
    public class SampleService : ISampleService
    {
        private readonly ISampleRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly INotification _notifications;

        public SampleService(
            ISampleRepository repository,
            IUnitOfWork unitOfWork,
            INotification notifications)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _notifications = notifications;
        }

        public SampleEntity GetSampleBy(int id)
        {
            try
            {
                _unitOfWork.BeginTransaction();

                var sample = _repository.GetById(id);

                _unitOfWork.Commit();

                return sample;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _notifications.AddMessage((int)HttpStatusCode.InternalServerError, ex.Message);
                return null;
            }
        }
    }
}
