using Microsoft.Extensions.DependencyInjection;

namespace Shared.Presentation.Filters;

/// <summary>
/// Extension methods for configuring global MVC filters.
/// </summary>
public static class FilterExtensions
{
    /// <summary>
    /// Adds the global exception and validation filters to the MVC options.
    /// Should be called in AddControllers configuration.
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddGlobalFilters(this IServiceCollection services)
    {
        services.AddScoped<ValidationFilter>();
        services.AddScoped<ExceptionFilter>();

        return services;
    }
}
