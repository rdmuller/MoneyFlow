using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MoneyFlow.Application.Behaviors;
using System.Reflection;

namespace MoneyFlow.Application;

public static class DependencyInjection
{
    public static void AddDependencyInjectionApplication(this IServiceCollection services)
    {
        AddValidators(services);
    }

    private static void AddValidators(IServiceCollection services) 
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        services.AddScoped<ValidationFilter>();
    }
}