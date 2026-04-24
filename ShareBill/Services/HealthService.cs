using Polly;
using ShareBill.Infrastructure.Database;

namespace ShareBill.Services

{
    public class HealthService
    {
        private readonly IDbConnectionFactory _dbFactory;
        private readonly ILogger<HealthService> _logger;
        private readonly IAsyncPolicy _reconRetryPolicy;

        public HealthService( IDbConnectionFactory dbFactory, ILogger<HealthService> logger, IAsyncPolicy reconRetryPolicy)
        {

            _dbFactory = dbFactory;
            _logger = logger;
            _reconRetryPolicy = reconRetryPolicy;
        }

        public async Task<bool> CanReachDatabase() 
        {
            try
            {
                _logger.LogInformation("Checking database connectivity...");
                return await _reconRetryPolicy.ExecuteAsync(async () =>
                {
                    await using var connection = _dbFactory.CreateConnection();

                    await connection.OpenAsync();

                    await using var cmb = new Npgsql.NpgsqlCommand("SELECT 1", connection);

                    var result = await cmb.ExecuteScalarAsync();

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
