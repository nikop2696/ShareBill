using FluentValidation;

namespace ShareBill.Shared.Infrastructure.Validators
{
    public static class ValidatorsSetup
    {
        public static WebApplicationBuilder AddValidator(this WebApplicationBuilder builder) 
        {
            builder.AddValidatorServices();
            return builder;
        }

        private static WebApplicationBuilder AddValidatorServices(this WebApplicationBuilder builder) 
        {
            builder.Services.AddValidatorsFromProgram();
            return builder;
        }

        private static IServiceCollection AddValidatorsFromProgram(this IServiceCollection services) 
        {
            services.AddValidatorsFromAssemblyContaining<Program>();
            return services;
        }
    }
}
