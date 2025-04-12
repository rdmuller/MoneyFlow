using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MoneyFlow.Common;
using MoneyFlow.Infra.DataAccess;
using MoneyFlow.Infra.Services;

namespace MoneyFlow.Infra;

public static class DependencyInjection
{
    public static void AddInfra(this IServiceCollection services, IConfiguration config)
    {
        AddDataBase(services, config);
        AddServices(services);
    }

    private static void AddServices(IServiceCollection services)
    {
        services.AddSingleton<IDateTimeProvider, DateTimeProvider>();
    }

    private static void AddDataBase(IServiceCollection services, IConfiguration config)
    {
        var connectionString = config.GetConnectionString("DefaultConnection");

        services.AddDbContext<ApplicationDbContext>((sp, options) =>
        {
            options.UseSqlServer(connectionString, b =>
            {
                b.MigrationsHistoryTable(HistoryRepository.DefaultTableName, DbSchemas.SYSTEM);
            });

            //options.AddInterceptors(sp.GetServices<ISaveChangesInterceptor>());
        });
    }
}