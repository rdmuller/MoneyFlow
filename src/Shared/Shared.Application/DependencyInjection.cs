using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Shared.Application.Behaviours;
using Shared.Application.Events;
using Shared.Application.Messaging;

namespace Shared.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services, Assembly[] assemblies)
    {
        services.Scan(scan => scan.FromAssemblies(assemblies)
            .AddClasses(classes => classes.AssignableTo(typeof(IQueryHandler<,>)), publicOnly: false)
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<>)), publicOnly: false)
                .AsImplementedInterfaces()
                .WithScopedLifetime()
            .AddClasses(classes => classes.AssignableTo(typeof(ICommandHandler<,>)), publicOnly: false)
                .AsImplementedInterfaces()
                .WithScopedLifetime()
        );

        services.Decorate(typeof(ICommandHandler<,>), typeof(LoggingDecorator.CommandHandler<,>));

        services.AddTransient<IDomainEventsDispatcher, DomainEventsDispatcher>();
        services.AddValidatorsFromAssemblies(assemblies, includeInternalTypes: true);

        return services;
    }

    //public static IServiceCollection AddMediator(this IServiceCollection services, params Assembly[] assemblies)
    //{
    //    services.AddTransient<IMediator, Mediator>();

    //    Type handlerType = typeof(IRequestHandler<,>);

    //    foreach (Assembly assembly in assemblies)
    //    {
    //        var handlers = assembly
    //            .GetTypes()
    //            .Where(type => !type.IsAbstract && !type.IsInterface)
    //            .SelectMany(x => x.GetInterfaces(), (t, i) => new { Type = t, Interface = i })
    //            .Where(t => t.Interface.IsGenericType && t.Interface.GetGenericTypeDefinition() == handlerType);

    //        foreach (var handler in handlers)
    //        {
    //            services.AddTransient(handler.Interface, handler.Type);
    //        }
    //    }

    //    return services;
    //}

    //private static void AddDomainEvents(IServiceCollection services)
    //{
    //    services.AddTransient<IDomainEventsDispatcher, DomainEventsDispatcher>();
    //    //services.AddScoped<IDomainEventHandler<UserChangePasswordDomainEvent>, SendEmailChangePassword>();

    //    Type domainEventHandlerType = typeof(IDomainEventHandler<>);
    //    var domainEventHandlers = Assembly.GetExecutingAssembly()
    //        .GetTypes()
    //        .Where(type => !type.IsAbstract && !type.IsInterface)
    //        .SelectMany(x => x.GetInterfaces(), (t, i) => new { Type = t, Interface = i })
    //        .Where(t => t.Interface.IsGenericType && t.Interface.GetGenericTypeDefinition() == domainEventHandlerType);

    //    foreach (var handler in domainEventHandlers)
    //    {
    //        services.AddTransient(handler.Interface, handler.Type);
    //    }
    //}
}
