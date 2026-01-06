using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MoneyFlow.Domain.Abstractions;
using MoneyFlow.Domain.General.Entities.Categories;
using MoneyFlow.Domain.General.Entities.Markets;
using MoneyFlow.Domain.General.Entities.Users;
using MoneyFlow.Domain.General.Security;
using MoneyFlow.Domain.General.Services.Email;
using MoneyFlow.Infra.DataAccess;
using MoneyFlow.Infra.DataAccess.Repositories;
using MoneyFlow.Infra.Services;
using MoneyFlow.Infra.Settings;
using SharedKernel.Services;

namespace MoneyFlow.Infra;

public static class DependencyInjection
{
    public static void AddInfra(this IServiceCollection services, IConfiguration config)
    {
        AddDataBase(services, config);
        AddServices(services);
        AddRepositories(services);
        AddToken(services, config);
        AddEmail(services, config);
    }

    private static void AddEmail(IServiceCollection services, IConfiguration config)
    {
        services.AddSingleton<EmailSettings>(EmailSettings.GetSettings(config));
    }

    private static void AddToken(IServiceCollection services, IConfiguration config)
    {
        var expirationTimeMinutes = config.GetValue<uint>("Settings:jwt:ExpiresMinutes");
        var signingKey = config.GetValue<string>("Settings:jwt:SigningKey");

        services.AddScoped<IAccessTokenGenerator>(config => new AccessTokenGenerator(expirationTimeMinutes, signingKey!, new DateTimeProvider()));
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
        services.AddScoped<IUserReadRepository, UserRepository>();
        services.AddScoped<IMarketReadRepository, MarketRepository>();
        services.AddScoped<IMarketWriteRepository, MarketRepository>();
        services.AddScoped<ICategoryReadRepository, CategoryRepository>();
        services.AddScoped<ICategoryWriteRepository, CategoryRepository>();
    }

    private static void AddServices(IServiceCollection services)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddScoped<ILoggedUser, LoggedUser>();
        services.AddTransient<IEmailService, EmailService>();
    }

    private static void AddDataBase(IServiceCollection services, IConfiguration config)
    {
        services.AddScoped<ISaveChangesInterceptor, EFCoreInterceptor>();

        var connectionString = config.GetConnectionString("DefaultConnection");

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseNpgsql(connectionString, config =>
            {
                config.MigrationsHistoryTable(HistoryRepository.DefaultTableName.ToLower(), DbSchemas.System);
                config.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
            }).UseSnakeCaseNamingConvention();
        });
    }
}
