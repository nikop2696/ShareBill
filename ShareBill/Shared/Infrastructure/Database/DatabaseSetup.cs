namespace ShareBill.Shared.Infrastructure.Database
{
    public static class DatabaseSetup
    {
        public static WebApplicationBuilder AddDatabaseConfiguration(this WebApplicationBuilder builder) 
        {
            builder.AddDbServices();
            return builder;
        }

        private static WebApplicationBuilder AddDbServices(this WebApplicationBuilder builder) 
        {
            builder.Services.AddDBFactory();
            return builder;
        }

        private static IServiceCollection AddDBFactory(this IServiceCollection services) 
        {
            services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();
            return services;
        }
    }
}
