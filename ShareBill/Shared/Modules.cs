using ShareBill.Modules.Health;
using ShareBill.Modules.Users;

namespace ShareBill.Shared
{
    public static class Modules
    {
        public static IServiceCollection AddModules(this IServiceCollection services)
        {
            services.AddHealthModule();
            services.AddUsersModule();
            
            return services;
        }
    }
}
