using Polly;
using ShareBill.Infrastructure.Database;
using ShareBill.Infrastructure.Policies;

namespace ShareBill.Services

{
    public class HealthService
    {
        private readonly IDbConnectionFactory _dbFactory;
        private readonly ILogger<HealthService> _logger;
        private readonly IRetryPolicies _retryPolicies;

        public HealthService( IDbConnectionFactory dbFactory, ILogger<HealthService> logger, IRetryPolicies retryPolicies)
        {

            _dbFactory = dbFactory;
            _logger = logger;
            _retryPolicies = retryPolicies;
        }

        public async Task<bool> CanReachDatabase() 
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

                    return true;
                });
            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Database connection failed");

                return false;
            }

            
        }
    }
}
