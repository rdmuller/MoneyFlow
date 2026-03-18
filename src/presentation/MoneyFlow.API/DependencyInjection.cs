using MoneyFlow.API.Security;
using MoneyFlow.Domain.Tenant.Services;

namespace MoneyFlow.API;

internal static class DependencyInjection
{
    public static void AddDependencyInjectionAPI(this IServiceCollection services)
    {
        services.AddScoped<JwtBearerEventsHandler>();
        services.AddScoped<ITenantProvider, TenantProvider>();
    }
}
