using Polly;
using ShareBill.DTOs.Responses;
using ShareBill.Errors;
using ShareBill.Infrastructure.Database;
using ShareBill.Infrastructure.Policies;

namespace ShareBill.Services

{
    public class HealthService
    {
     
        private readonly IDbConnectionFactory _dbFactory;
        private readonly ILogger<HealthService> _logger;
        private readonly IRetryPolicies _retryPolicies;

        public HealthService(IDbConnectionFactory dbFactory, ILogger<HealthService> logger, IRetryPolicies retryPolicies)
        {
            
            _dbFactory = dbFactory;
            _logger = logger;
            _retryPolicies = retryPolicies;
        }

        public async Task<OperationResult<bool>> CanReachDatabase() 
        {
            try
            {
                _logger.LogInformation("Checking database connectivity...");
                return await _retryPolicies.DBRetryPolicy.ExecuteAsync(async () =>
                {
                    await using var connection = _dbFactory.CreateConnection();

                    await connection.OpenAsync();

                    await using var cmb = new Npgsql.NpgsqlCommand("SELECT 1", connection);

                    await cmb.ExecuteScalarAsync();

                    _logger.LogInformation("Database is reachable");

                    return OperationResult<bool>.Ok(true, "Database is reachable");
                });
            }
            catch (Exception ex) 
            {
                var(level, payload) = ex.ToLog();
                _logger.Log(level, ex, "Database connection failed. {@Payload}", payload);

                return OperationResult<bool>.Fail("Database connection failed");
            }

            
        }
    }
}
