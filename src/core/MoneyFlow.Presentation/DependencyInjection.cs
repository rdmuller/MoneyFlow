using MoneyFlow.Domain.Tenant.Services;
using MoneyFlow.Presentation.Security;

namespace MoneyFlow.Presentation;

internal static class DependencyInjection
{
    public static void AddDependencyInjectionAPI(this IServiceCollection services)
    {
        services.AddScoped<JwtBearerEventsHandler>();
        services.AddScoped<ITenantProvider, TenantProvider>();
    }
}
