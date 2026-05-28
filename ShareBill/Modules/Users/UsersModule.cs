using ShareBill.Modules.Users.Application;
using ShareBill.Modules.Users.Infrastructure;

namespace ShareBill.Modules.Users
{
    public static class UsersModule
    {
        public static IServiceCollection AddUsersModule(this IServiceCollection services)
        {
            services.AddScoped<ISignUpUserService, SignUpUserService>();
            services.AddScoped<ISignInUserService, UserSignInService>();

            return services;
        }
    }
}
