using Npgsql;

namespace ShareBill.Shared.Infrastructure.Database
{
    public interface IDbConnectionFactory
    {

        NpgsqlConnection CreateConnection();
    }
}
