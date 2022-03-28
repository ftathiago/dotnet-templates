using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using Npgsql;
using WebApi.DapperInfraData.Contexts;
using Xunit;

namespace WebApi.DapperInfraData.Tests.Contexts
{
    public class NpgConnectionFactoryTests
    {
        private const string ConnectionStringTest =
            "Server=myServerAddress;Database=myDataBase;User Id=myUsername;Password=myPassword;";

        private readonly Mock<IConfiguration> _configuration;

        public NpgConnectionFactoryTests() =>
            _configuration = new Mock<IConfiguration>(MockBehavior.Strict);

        [Fact]
        public void ShouldReturnASqlConnection()
        {
            // Given
            var defaultSection = new Mock<IConfigurationSection>();
            defaultSection
                .SetupGet(m => m[It.Is<string>(s => s == "Default")])
                .Returns(ConnectionStringTest);
            _configuration
                .Setup(a => a.GetSection(It.Is<string>(s => s == "ConnectionStrings")))
                .Returns(defaultSection.Object);

            var factory = new NpgConnectionFactory(_configuration.Object);

            // When
            var conn = factory.GetNewConnection() as NpgsqlConnection;

            // Then
            conn.Should().NotBeNull();
        }
    }
}
