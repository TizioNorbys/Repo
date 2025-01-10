using System.Text;
using StocksApi.Stocks;
using StocksApi.Persistence;
using Microsoft.EntityFrameworkCore;
using StocksApi.Persistence.Entities;
using StocksApi.OptionsSetup;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using StocksApi.Abstractions;
using StocksApi.Authentication;
using StocksApi.RealTime;
using Microsoft.AspNetCore.SignalR;
using StocksApi.RealTime.SignalR;
using StocksApi.Persistence.Repositories.Base;
using StocksApi.Persistence.Repositories;
using Microsoft.OpenApi.Models;
using Microsoft.OpenApi.Any;
using StocksApi.Validators;
using FluentValidation;
using StocksApi.Serialization;
using StocksApi.DTOs;
using StocksApi.ErrorHandling;

namespace StocksApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers()
            .AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.WriteIndented = true;
                options.JsonSerializerOptions.Converters.Add(new DateOnlyJsonConverter(builder.Configuration));
            });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.MapType<DateOnly>(() => new OpenApiSchema
            {
                Type = "string",
                Format = "date",
                Example = new OpenApiString(DateTime.Today.ToString(builder.Configuration["Json:Serializer:DateTimeFormat"]))
            });
        });

        builder.Services.AddProblemDetails();

        // EF Core configuration
        builder.Services.AddDbContext<AppDbContext>(options =>
        {
            var connectionString = builder.Configuration.GetConnectionString("Default");
            options.UseSqlite(connectionString);
        });

        // Identity configuration
        builder.Services.AddDefaultIdentity<AppUser>(options =>
        {
            options.User.RequireUniqueEmail = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireDigit = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Password.RequiredLength = 8;
        })
        .AddEntityFrameworkStores<AppDbContext>();

        // Jwt configuration
        builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                var jwtSection = builder.Configuration.GetSection("Jwt");
          
                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateIssuerSigningKey = true,
                    ValidateLifetime = true,
                    ValidIssuer = jwtSection.GetValue<string>("Issuer"),
                    ValidAudience = jwtSection.GetValue<string>("Audience"),
                    IssuerSigningKey = new SymmetricSecurityKey
                        (Encoding.UTF8.GetBytes(jwtSection.GetValue<string>("SecretKey")!))
                };
            });

        // Typed client
        builder.Services.AddHttpClient<StocksClient>(httpClient =>
        {
            httpClient.BaseAddress = new Uri("https://finnhub.io/api/v1");
        });

        // Background services
        builder.Services.AddHostedService<StocksPriceUpdater>();

        // SignalR
        builder.Services.AddSignalR();
        builder.Services.AddSingleton<IUserIdProvider, CustomUserIdProvider>();

        // Fluent validation
        builder.Services.AddScoped<IValidator<HistoricalDataDto>, HistoricalDataDtoValidator>();

        // Options setup
        builder.Services.ConfigureOptions<JwtOptionsSetup>();

        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();

        builder.Services.AddScoped<IJwtProvider, JwtProvider>();
        builder.Services.AddScoped<IStockRepository, StockRepository>();
        builder.Services.AddScoped<StocksService>();
        builder.Services.AddSingleton<SymbolsManager>();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
            app.UseDeveloperExceptionPage();
        }

        app.UseExceptionHandler();

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.MapHub<StocksHub>("/stocks-hub");

        app.Run();
    }
}