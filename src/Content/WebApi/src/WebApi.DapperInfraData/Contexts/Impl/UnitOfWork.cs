using Microsoft.Extensions.Logging;
using System;
using System.Data;
using WebApi.Shared.Data.Contexts;
using WebApi.Shared.Exceptions;

namespace WebApi.DapperInfraData.Contexts
{
    public class UnitOfWork : IUnitOfWork, IDisposable
    {
        private readonly ILogger<UnitOfWork> _logger;
        private readonly IDbConnectionFactory _connFactory;
        private int _transactionCounter = 0;
        private IDbConnection _connection;
        private bool _disposedValue;

        public UnitOfWork(ILogger<UnitOfWork> logger, IDbConnectionFactory connFactory)
        {
            _logger = logger;
            _connFactory = connFactory;
        }

        public IDbConnection Connection => _connection ??= _connFactory.GetNewConnection();

        public IDbTransaction Transaction { get; protected set; }

        public void BeginTransaction()
        {
            if (_transactionCounter == 0)
            {
                Connection.Open();
                Transaction = Connection.BeginTransaction();
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

            if (Connection is not null)
            {
                Connection.Close();
                Connection.Dispose();
            }
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
            Transaction?.Dispose();
            Transaction = null;
            if (_connection?.State == ConnectionState.Open)
            {
                _connection.Close();
            }
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
