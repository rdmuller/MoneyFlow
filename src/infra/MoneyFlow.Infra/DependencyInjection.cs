using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MoneyFlow.Common.Services;
using MoneyFlow.Domain.Repositories;
using MoneyFlow.Domain.Repositories.Users;
using MoneyFlow.Domain.Security;
using MoneyFlow.Infra.DataAccess;
using MoneyFlow.Infra.DataAccess.Repositories;
using MoneyFlow.Infra.Services;

namespace MoneyFlow.Infra;

public static class DependencyInjection
{
    public static void AddInfra(this IServiceCollection services, IConfiguration config)
    {
        AddDataBase(services, config);
        AddServices(services);
        AddRepositories(services);
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