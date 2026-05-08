using ShareBill.Modules.Users.Application;

namespace ShareBill.Modules.Users
{
    public static class UsersModule
    {
        public static IServiceCollection AddUsersModule(this IServiceCollection services)
        {
            services.AddScoped<SignUpUserService>();

            return services;
        }
    }
}
