using Microsoft.Extensions.Options;
using ShareBill.Configurators;
using Supabase;

namespace ShareBill.Infrastructure.DI
{
    public static class SupabaseExtensions
    {
                public static IServiceCollection AddSupabase(this IServiceCollection services)
        {
           
            services.AddSingleton<Client>(sp =>
            {
                var setting = sp.GetRequiredService<IOptions<SupabaseSettings>>().Value;

                if (string.IsNullOrWhiteSpace(setting.Key))
                {
                    throw new Exception("Supabase Key is NULL or EMPTY");
                }

                if (string.IsNullOrWhiteSpace(setting.Url))
                {
                    throw new Exception("Supabase Url is NULL or EMPTY");
                }


                var options = new SupabaseOptions
                {
                    AutoRefreshToken = true,
                    AutoConnectRealtime = true,
                };

                var client = new Supabase.Client(setting.Url, setting.Key, options);
                client.InitializeAsync().GetAwaiter().GetResult();
                return client;
            });
            return services;
        }
    }
}
