using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;
using System;
using System.Data;
using System.Diagnostics.CodeAnalysis;
using WebApi.Shared.Data.Contexts;
using WebApi.Shared.Exceptions;

namespace WebApi.EfInfraData.Contexts
{
    [ExcludeFromCodeCoverage]
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ILogger<UnitOfWork> _logger;
        private readonly WebApiDbContext _context;
        private int _transactionCounter = 0;
        private bool _disposedValue;
        private IDbContextTransaction _entityTransaction;

        public UnitOfWork(ILogger<UnitOfWork> logger, WebApiDbContext context)
        {
            _logger = logger;
            _context = context;
        }

        public IDbConnection Connection => _context.Database.GetDbConnection();

        public IDbTransaction Transaction => _entityTransaction?.GetDbTransaction();

        public void BeginTransaction()
        {
            if (_transactionCounter == 0)
            {
                _entityTransaction = _context.Database.BeginTransaction();
            }

            _transactionCounter++;
        }

        public void Commit()
        {
            try
            {
                TryCommit();
            }
            catch (NotOpenTransactionException)
            {
                throw;
            }
            catch (Exception)
            {
                Rollback();
                throw;
            }
        }

        public void Rollback()
        {
            ValidateTransactionIsOpen();

            _transactionCounter = 0;
            Transaction?.Rollback();
            ClearTransaction();
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposedValue)
            {
                return;
            }

            if (!disposing)
            {
                _disposedValue = true;
            }

            LogLostTransaction();
        }

        private void TryCommit()
        {
            ValidateTransactionIsOpen();

            _transactionCounter--;
            if (_transactionCounter > 0)
            {
                return;
            }

            Transaction.Commit();

            ClearTransaction();
        }

        private void ValidateTransactionIsOpen()
        {
            if (Transaction is null || _transactionCounter < 0)
            {
                throw new NotOpenTransactionException("Commit");
            }
        }

        private void ClearTransaction()
        {
            _entityTransaction?.Dispose();
            _entityTransaction = null;
        }

        private void LogLostTransaction()
        {
            if (_transactionCounter == 0)
            {
                return;
            }

            _logger.LogWarning("There's a transaction pedding!");
        }
    }
}
