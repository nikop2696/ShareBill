using Asp.Versioning;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using Polly;
using Scalar.AspNetCore;
using Serilog;
using Serilog.Core;
using ShareBill.Configurators;
using ShareBill.Infrastructure.Database;
using ShareBill.Infrastructure.DI;
using ShareBill.Infrastructure.Policies;
using ShareBill.LoggerConfigurators;
using ShareBill.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//Swagger Configuration
builder.Services.AddEndpointsApiExplorer();


// Supabase configuration
builder.Services.Configure<SupabaseSettings>(
    builder.Configuration.GetSection("Supabase"));


builder.Services.AddSupabase();

builder.Services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();

builder.Services.AddScoped<HealthService>();

builder.Services.AddScoped<SignUpUserService>();

builder.Services.AddSingleton<IRetryPolicies, RetryPolicesProvider>();

builder.Services.AddSingleton(ConfiguredLogger.BaseLogger());



builder.Services.AddApiVersioning(options =>
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

// Add API Explorer to support versioning in Swagger


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();

}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
