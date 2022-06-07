using Czertainly.Auth.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using Czertainly.Auth.Data.Contracts;
using Czertainly.Auth.Data.Repositiories;
using Czertainly.Auth.Services;
using Czertainly.Auth.Common.Filters;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(swagger =>
{
    swagger.SwaggerDoc("1.0", new OpenApiInfo { Title = "Czertainly Auth Service", Version = "1.0" });
});

builder.Services.AddApiVersioning(o =>
{
    o.AssumeDefaultVersionWhenUnspecified = true;
    o.DefaultApiVersion = new ApiVersion(1, 0);
    o.ReportApiVersions = true;
});

builder.Services.AddDbContext<AuthDbContext>(opts => {
    opts.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"), pgsqlOpts =>
    {
        pgsqlOpts.MigrationsHistoryTable("_migrations_history", "auth");
    });
});

// add app services
builder.Services.AddScoped<IRepositoryManager, RepositoryManager>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ValidationFilter>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
