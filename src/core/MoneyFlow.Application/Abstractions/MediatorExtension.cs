using Microsoft.Extensions.DependencyInjection;
using SharedKernel.Mediator;
using System.Reflection;

namespace MoneyFlow.Application.Abstractions;

internal static class MediatorExtension
{
    public static IServiceCollection AddMediator(this IServiceCollection services, params Assembly[] assemblies)
    {
        services.AddTransient<IMediator, Mediator>();

        var handlerType = typeof(IRequestHandler<,>);

        foreach (var assembly in assemblies)
        {
            var handlers = assembly
                .GetTypes()
                .Where(type => !type.IsAbstract && !type.IsInterface)
                .SelectMany(x => x.GetInterfaces(), (t, i) => new { Type = t, Interface = i })
                .Where(t => t.Interface.IsGenericType && t.Interface.GetGenericTypeDefinition() == handlerType);

            foreach (var handler in handlers)
            {
                services.AddTransient(handler.Interface, handler.Type);
            }
        }

        return services;
    }
}