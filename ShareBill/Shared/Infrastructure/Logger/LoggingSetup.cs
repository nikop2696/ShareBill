using Serilog;
using ShareBill.Shared.Logger;

namespace ShareBill.Shared.Infrastructure.Logger
{
    public static class LoggingSetup
    {
        public static WebApplicationBuilder AddLogger(this WebApplicationBuilder builder) 
        {
            builder.ConfigureLoggerHost();
            builder.AddLoggerService();
            return builder;
        }

        private static WebApplicationBuilder ConfigureLoggerHost(this WebApplicationBuilder builder) 
        {
            builder.Host.UseLogger();
            return builder;
        }

        private static IHostBuilder UseLogger(this IHostBuilder host)
        {
            host.UseSerilog();

            return host;
        }
        private static WebApplicationBuilder AddLoggerService(this WebApplicationBuilder builder) 
        {
            builder.Services.AddConfiguredLogger();
            return builder;
        }

        private static IServiceCollection AddConfiguredLogger(this IServiceCollection services) 
        {
            services.AddSingleton(ConfiguredLogger.BaseLogger());
            return services;
        }
    }
}
