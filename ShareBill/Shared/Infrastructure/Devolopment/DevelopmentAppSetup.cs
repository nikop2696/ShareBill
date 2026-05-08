using Scalar.AspNetCore;

namespace ShareBill.Shared.Infrastructure.Devolopment
{
    public static class DevelopmentAppSetup
    {
        public static WebApplication ConfigureDevelopmentApp(this WebApplication app)
        {
            if (app.Environment.IsDevelopment())
            {
                app.UseScalarApi();
            }
            return app;
        }

        private static WebApplication UseScalarApi(this WebApplication app)
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
            return app;

        }
    }
}
