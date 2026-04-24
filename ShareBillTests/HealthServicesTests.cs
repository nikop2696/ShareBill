using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using Npgsql;
using Polly;
using Serilog;
using ShareBill.Infrastructure.Database;
using ShareBill.Infrastructure.Policies;
using ShareBill.Services;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Xunit;


namespace ShareBillTests
{
    public class HealthServicesTests
    {
        

        [Fact]
        public async Task HealthCheck_Should_Reach_Supabase()
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddJsonFile("appsettings.Development.json", optional: true)
                .Build();

            var factory = new DbConnectionFactory(config);

            var logger = new Mock<ILogger<HealthService>>();

            var policy = RetryPolices.GetDBRetryPolicy();

            var service = new HealthService(factory, logger.Object, policy);

            var result = await service.CanReachDatabase();
            Assert.True(result);
            

        }

        [Fact]
        public async Task HealthCheck_Should_ReturnFalse_When_DbIsDown()
        {
            var factoryMock = new Mock<IDbConnectionFactory>();
            var logger = new Mock<ILogger<HealthService>>();

            factoryMock
                .Setup(x => x.CreateConnection())
                .Throws(new NpgsqlException());


            var policy = Policy
                .Handle<Exception>()
                .RetryAsync(1);

            var service = new HealthService(factoryMock.Object, logger.Object, policy);

            var result = await service.CanReachDatabase();

            Assert.False(result);
        }
    }
}
