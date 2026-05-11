using Asp.Versioning;
using Scalar.AspNetCore;
using Serilog;
using ShareBill.Shared;
using ShareBill.Shared.Infrastructure.Api;
using ShareBill.Shared.Infrastructure.Devolopment;
using ShareBill.Shared.Infrastructure.SupaBase;


var builder = WebApplication.CreateBuilder(args);


// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi




// Shared services

builder.AddSharedToBuilder();

//Add Modules
builder.Services.AddModules();



builder.Services.AddHttpContextAccessor();

// Add API Explorer to support versioning in Swagger



var app = builder.Build();

// Configure the HTTP request pipeline.
app.AddSharedToApp();

app.Run();
