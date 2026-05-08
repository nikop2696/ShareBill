using Asp.Versioning;
using ShareBill.Shared.Infrastructure.Api;

namespace ShareBill.Shared.Infrastructure.Api
{
    public static class ApiSetup
    {
        public static WebApplicationBuilder AddApi(this WebApplicationBuilder builder)
        {
            builder.AddApiServices();
            return builder;
        }

        private static WebApplicationBuilder AddApiServices(this WebApplicationBuilder builder)
        {
            builder.Services.AddOpenApi();
            builder.Services.AddApiVersioning();
            builder.Services.AddEndpointsApiExplorer();
            return builder;
        }


        private static IServiceCollection AddApiVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(options =>
            {
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new Asp.Versioning.ApiVersion(1, 0);
                options.ReportApiVersions = true;

                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            })
            // Add API Explorer to support versioning in Swagger
            .AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";
                options.SubstituteApiVersionInUrl = true;
            });

            return services;
        }
    }
}
