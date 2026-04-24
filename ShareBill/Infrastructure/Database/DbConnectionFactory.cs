using Npgsql;

namespace ShareBill.Infrastructure.Database
{
    public class DbConnectionFactory :IDbConnectionFactory
    {
        private readonly string _connectionString;

        public DbConnectionFactory(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Default") ??
                throw new InvalidOperationException(
                    "Connection string 'Default' is missing or empty."); 

            
        }

        public NpgsqlConnection CreateConnection()
        {
            return new NpgsqlConnection(_connectionString);
        }
    }
}
