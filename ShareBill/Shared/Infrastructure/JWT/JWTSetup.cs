using Microsoft.AspNetCore.Authentication.JwtBearer;
using Supabase;

namespace ShareBill.Shared.Infrastructure.JWT
{
    public static class JWTSetup
    {

        public static WebApplicationBuilder AddJWT(this WebApplicationBuilder builder)
        {
            builder.Services.Configure<JWTSettings>(builder.Configuration.GetSection("JWT"));

            var jwtSettings = builder.Configuration.GetSection("JWT").Get<JWTSettings>();

            if (jwtSettings == null) 
            {
                throw new ArgumentNullException(nameof(jwtSettings));
            }

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.Authority = jwtSettings.Authority;

                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtSettings.Authority,

                        ValidateAudience = false,
                        ValidateLifetime = true,

                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            var token = context.Request.Headers["Authorization"].FirstOrDefault();
                            if (token?.StartsWith("Bearer ") == true)
                            {
                                context.Token = token.Substring("Bearer ".Length).Trim();
                            }
                            return Task.CompletedTask;
                        }
                    };
                });



           
            return builder;
        }
    }
}
