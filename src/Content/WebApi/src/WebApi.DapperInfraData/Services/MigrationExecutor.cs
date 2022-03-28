using FluentMigrator.Runner;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using WebApi.WarmUp.Abstractions;

namespace WebApi.DapperInfraData.Services
{
    [ExcludeFromCodeCoverage]
    public class MigrationExecutor : IWarmUpCommand
    {
        private readonly IMigrationRunner _migrationRunner;
        private readonly ILogger<MigrationExecutor> _logger;
        private readonly IHostApplicationLifetime _applicationLifetime;

        public MigrationExecutor(
            IMigrationRunner migrationRunner,
            ILogger<MigrationExecutor> logger,
            IHostApplicationLifetime applicationLifetime)
        {
            _migrationRunner = migrationRunner;
            _logger = logger;
            _applicationLifetime = applicationLifetime;
        }

        public async Task Execute()
        {
            try
            {
                // Avoid Exception: schema duplication.
                await Task.Delay(1000);

                _logger.LogInformation("Migration runner started at {UtcNow}", DateTimeOffset.UtcNow);
                _migrationRunner.ValidateVersionOrder();
                _migrationRunner.MigrateUp();
                _logger.LogInformation("Migration runner completed at {UtcNow}", DateTimeOffset.UtcNow);
            }
            catch (Exception exception)
            {
                _logger.LogCritical(exception, "Error while executing migrations: {Message}", exception.Message);
                _applicationLifetime.StopApplication();
                throw;
            }
        }
    }
}
