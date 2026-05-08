namespace ShareBill.Shared.Infrastructure.Policies
{
    public static class PolicesSetup
    {
        public static WebApplicationBuilder AddPolices(this WebApplicationBuilder builder) 
        {
            builder.AddPolicesServices();
            return builder;
        }

        private static WebApplicationBuilder AddPolicesServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddPolicesProvider();
            return builder;
        }

        private static IServiceCollection AddPolicesProvider(this IServiceCollection services) 
        {
            services.AddSingleton<IRetryPolicies, RetryPolicesProvider>();
            return services;
        }
            
    }
}
