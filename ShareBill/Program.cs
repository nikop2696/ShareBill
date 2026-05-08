using Asp.Versioning;
using Scalar.AspNetCore;
using Serilog;
using ShareBill.Shared;
using ShareBill.Shared.Infrastructure.Api;
using ShareBill.Shared.Infrastructure.Devolopment;
using ShareBill.Shared.Infrastructure.SupaBase;


var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//Add Api 

builder.AddApi();

//Add Supabase

builder.AddSupabase();


// Shared services

builder.AddShared();

//Add Modules
builder.Services.AddModules();



builder.Services.AddHttpContextAccessor();

// Add API Explorer to support versioning in Swagger



var app = builder.Build();

// Configure the HTTP request pipeline.
app.ConfigureDevelopmentApp();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
