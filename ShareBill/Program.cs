using ShareBill.Services;
using Serilog;
using Serilog.Core;
using ShareBill.LoggerConfigurators;
using ShareBill.Infrastructure.Policies;
using ShareBill.Infrastructure.Database;
using Polly;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<IDbConnectionFactory, DbConnectionFactory>();
builder.Services.AddScoped<HealthService>();
builder.Services.AddSingleton<IAsyncPolicy>(sp => RetryPolices.GetDBRetryPolicy());
builder.Services.AddSingleton(ConfiguredLogger.BaseLogger());


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
