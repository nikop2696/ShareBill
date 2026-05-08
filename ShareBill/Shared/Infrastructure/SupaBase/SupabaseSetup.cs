using Microsoft.Extensions.Options;
using Supabase;

namespace ShareBill.Shared.Infrastructure.SupaBase
{
    public static class SupabaseSetup
    {
        public static WebApplicationBuilder AddSupabase(this WebApplicationBuilder builder)
        {
            builder.ConfigureSupabase();
            builder.AddSupabaseServices();
            return builder;
            
        }

        private static WebApplicationBuilder ConfigureSupabase(this WebApplicationBuilder builder) 
        {
            builder.Services.Configure<SupabaseSettings>(
                builder.Configuration.GetSection("Supabase"));
            return builder;
        }

        private static WebApplicationBuilder AddSupabaseServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddClient();
            return builder;
        }

        private static IServiceCollection AddClient(this IServiceCollection services) 
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
