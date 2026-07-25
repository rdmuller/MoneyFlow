using MoneyFlow.Domain.General.Enums;

namespace MoneyFlow.API.Security;

internal static class Policies
{
    public static void AddAuthorizationPolicies(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(Roles.ADMIN, policy => policy.RequireRole(Roles.ADMIN));
            options.AddPolicy(Roles.USER, policy => policy.RequireRole(Roles.USER));
            //options.AddPolicy(Roles.ADMIN_OR_USER, policy => policy.RequireRole(Roles.ADMIN, Roles.USER));
            options.AddPolicy(Roles.ADMIN_OR_USER, policy => policy.RequireAssertion(context =>
                context.User.IsInRole(Roles.ADMIN) || 
                context.User.IsInRole(Roles.USER)));
        });
    }
}