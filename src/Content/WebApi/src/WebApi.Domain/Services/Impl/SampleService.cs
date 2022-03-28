using System;
using WebApi.Domain.Entities;
using WebApi.Domain.Exceptions;
using WebApi.Domain.Notifications;
using WebApi.Domain.Repositories;
using WebApi.Shared.Data.Contexts;

namespace WebApi.Domain.Services
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
            _unitOfWork.BeginTransaction();
            try
            {
                var sample = _repository.GetById(id);

                _unitOfWork.Commit();

                return sample;
            }
            catch (Exception ex)
            {
                _unitOfWork.Rollback();
                _notifications.AddException(ex, ErrorCode.PersistingError);
                return null;
            }
        }
    }
}
