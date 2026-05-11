using FluentValidation;
using ShareBill.Shared.Logger;
using ShareBill.Shared.Infrastructure.Database;
using ShareBill.Shared.Infrastructure.JWT;
using ShareBill.Shared.Infrastructure.Policies;
using ShareBill.Shared.Infrastructure.Api;
using ShareBill.Shared.Infrastructure.Logger;
using ShareBill.Shared.Infrastructure.SupaBase;
using ShareBill.Shared.Infrastructure.Validators;
using ShareBill.Shared.Infrastructure.Devolopment;

namespace ShareBill.Shared
{
    public static class Shared
    {
        public static WebApplicationBuilder AddSharedToBuilder(this WebApplicationBuilder builder)
        {

            builder.AddApi();
            builder.AddDatabaseConfiguration();
            builder.AddJWT();
            builder.AddLogger();
            builder.AddPolices();
            builder.AddSupabase();
            builder.AddValidator();

            return builder;
        }

        public static WebApplication AddSharedToApp(this WebApplication app)
        {
            app.ConfigureDevelopmentApp();
            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();

            return app;
        }

    }
}
