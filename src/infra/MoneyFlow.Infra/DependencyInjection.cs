using System.ComponentModel;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MoneyFlow.Common.Services;
using MoneyFlow.Domain.Repositories;
using MoneyFlow.Domain.Repositories.Users;
using MoneyFlow.Domain.Security;
using MoneyFlow.Domain.Services.Email;
using MoneyFlow.Infra.DataAccess;
using MoneyFlow.Infra.DataAccess.Repositories;
using MoneyFlow.Infra.Services;
using MoneyFlow.Infra.Settings;

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
        services.AddScoped<IUserQueryRepository, UserRepository>();
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
        services.AddScoped<ISaveChangesInterceptor, AuditableEntityInterceptor>();

        var connectionString = config.GetConnectionString("DefaultConnection");

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseSqlServer(connectionString, b =>
            {
                b.MigrationsHistoryTable(HistoryRepository.DefaultTableName, DbSchemas.SYSTEM);
                b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
            });
        });
    }
}
