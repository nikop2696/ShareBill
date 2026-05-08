using FluentValidation;
using ShareBill.Shared.Logger;
using ShareBill.Shared.Infrastructure.Database;
using ShareBill.Shared.Infrastructure.JWT;
using ShareBill.Shared.Infrastructure.Policies;
using ShareBill.Shared.Infrastructure.Api;

namespace ShareBill.Shared
{
    public static class Shared
    {
        public static WebApplicationBuilder AddShared(this WebApplicationBuilder builder)
        {

            builder.AddApi();
            builder.AddDatabaseConfiguration();
            builder.AddPolices();
            builder.AddJWT();

            return builder;
        }

        private static IServiceCollection AddSharedServices(this IServiceCollection services)
        {
            services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();
            services.AddSingleton<IRetryPolicies, RetryPolicesProvider>();
            services.AddSingleton(ConfiguredLogger.BaseLogger());
            services.AddValidatorsFromAssemblyContaining<Program>();
            return services;
        }
    }
}
