using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MoneyFlow.Domain.Abstractions.DataAccess;
using MoneyFlow.Domain.General.Entities.Categories;
using MoneyFlow.Domain.General.Entities.Currencies;
using MoneyFlow.Domain.General.Entities.Markets;
using MoneyFlow.Domain.General.Entities.Sectors;
using MoneyFlow.Domain.General.Entities.Users;
using MoneyFlow.Domain.General.Security;
using MoneyFlow.Domain.General.Services.Email;
using MoneyFlow.Infra.DataAccess;
using MoneyFlow.Infra.Repositories;
using MoneyFlow.Infra.Services;
using MoneyFlow.Infra.Settings;
using Shared.Application.Clock;
using Shared.Application.Messaging;

namespace MoneyFlow.Infra;

public static class DependencyInjection
{
    public static void AddMoneyFlowModule(this IServiceCollection services, IConfiguration config)
    {
        AddDataBase(services, config);
        AddServices(services);
        AddRepositories(services);
        AddToken(services, config);
        AddEmail(services, config);
        AddDomainEvents(services);
    }

    private static void AddDomainEvents(IServiceCollection services)
    {
        //services.AddTransient<IDomainEventsDispatcher, DomainEventsDispatcher>();

        Type domainEventHandlerType = typeof(IDomainEventHandler<>);
        var domainEventHandlers = Assembly.GetExecutingAssembly()
            .GetTypes()
            .Where(type => !type.IsAbstract && !type.IsInterface)
            .SelectMany(x => x.GetInterfaces(), (t, i) => new { Type = t, Interface = i })
            .Where(t => t.Interface.IsGenericType && t.Interface.GetGenericTypeDefinition() == domainEventHandlerType);

        foreach (var handler in domainEventHandlers)
        {
            services.AddTransient(handler.Interface, handler.Type);
        }
    }

    private static void AddEmail(IServiceCollection services, IConfiguration config)
    {
        services.AddSingleton<EmailSettings>(EmailSettings.GetSettings(config));
    }

    private static void AddToken(IServiceCollection services, IConfiguration config)
    {
        uint expirationTimeMinutes = config.GetValue<uint>("Settings:jwt:ExpiresMinutes");
        string? signingKey = config.GetValue<string>("Settings:jwt:SigningKey");

        services.AddScoped<IAccessTokenGenerator>(config => new AccessTokenGenerator(expirationTimeMinutes, signingKey!, new DateTimeProvider()));
    }

    private static void AddRepositories(IServiceCollection services)
    {
        services.AddScoped<IUserWriteOnlyRepository, UserRepository>();
        services.AddScoped<IUserReadRepository, UserRepository>();
        services.AddScoped<IMarketReadRepository, MarketRepository>();
        services.AddScoped<IMarketWriteRepository, MarketRepository>();
        services.AddScoped<ICategoryReadRepository, CategoryRepository>();
        services.AddScoped<ICategoryWriteRepository, CategoryRepository>();
        services.AddScoped<ISectorWriteRepository, SectorRepository>();
        services.AddScoped<ISectorReadRepository, SectorRepository>();
        services.AddScoped<ICurrencyWriteRepository, CurrencyRepository>();
        services.AddScoped<ICurrencyReadRepository, CurrencyRepository>();
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

        string? connectionString = config.GetConnectionString("DefaultConnection");

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
            options.UseNpgsql(connectionString, config =>
            {
                config.MigrationsHistoryTable(HistoryRepository.DefaultTableName.ToLower(), DbSchemas.System);
                config.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
            }).UseSnakeCaseNamingConvention();
        });

        services.AddScoped<IUnitOfWork>(s => s.GetRequiredService<ApplicationDbContext>());

    }
}
