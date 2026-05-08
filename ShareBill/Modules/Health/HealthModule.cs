using System.Runtime.CompilerServices;

namespace ShareBill.Modules.Health
{
    public static class HealthModule
    {
        public static IServiceCollection AddHealthModule(this IServiceCollection services)
        {
            services.AddScoped<Application.HealthService>();

            
            return services;
        }
    }
}
