using Npgsql;

namespace ShareBill.Infrastructure.Database
{
    public interface IDbConnectionFactory
    {
        NpgsqlConnection CreateConnection();
    }
}
