using System.Reflection;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using MoneyFlow.Application.Common.Events;

namespace MoneyFlow.Application;

public static class DependencyInjection
{
    public static void AddDependencyInjectionApplication(this IServiceCollection services)
    {
        AddValidators(services);
        AddDomainEvents(services);
    }

    private static void AddValidators(IServiceCollection services) 
    {
        services.AddValidatorsFromAssembly(typeof(DependencyInjection).Assembly);
        //services.AddScoped<ValidationFilter>();
    }

    private static void AddDomainEvents(IServiceCollection services)
    {
        services.AddTransient<IDomainEventsDispatcher, DomainEventsDispatcher>();
        //services.AddScoped<IDomainEventHandler<UserChangePasswordDomainEvent>, SendEmailChangePassword>();
        
        var domainEventHandlerType = typeof(IDomainEventHandler<>);
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
}