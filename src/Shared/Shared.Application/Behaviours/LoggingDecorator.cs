using System.Diagnostics;
using Microsoft.Extensions.Logging;
using Serilog.Context;
using Shared.Application.Messaging;
using Shared.Domain;

namespace Shared.Application.Behaviours;

internal static class LoggingDecorator
{
    internal sealed class CommandHandler<TCommand, TResponse>(
        ICommandHandler<TCommand, TResponse> innerHandler,
        ILogger<CommandHandler<TCommand, TResponse>> logger)
        : ICommandHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
    {
        public async Task<Result<TResponse>> HandleAsync(TCommand request, CancellationToken cancellationToken = default)
        {
            string moduleName = GetModuleName(typeof(TCommand).FullName!);
            string commandName = typeof(TCommand).Name!;

            Result<TResponse> result = await innerHandler.HandleAsync(request, cancellationToken);

            Activity.Current?.AddTag("request.module", moduleName);
            Activity.Current?.AddTag("request.command", commandName);

            if (result.IsSuccess)
                logger.LogInformation("Finished command {Command} successfully", commandName);
            else
            {
                using (LogContext.PushProperty("Errors", result.Errors, true))
                {
                    logger.LogError("Finished command {Command} with errors", commandName);
                }
            }

            return result;
        }
    }

    internal sealed class CommandHandler<TCommand>(
        ICommandHandler<TCommand> innerHandler,
        ILogger<CommandHandler<TCommand>> logger)
        : ICommandHandler<TCommand>
        where TCommand : ICommand
    {
        public async Task<Result> HandleAsync(TCommand request, CancellationToken cancellationToken = default)
        {
            string moduleName = GetModuleName(typeof(TCommand).FullName!);
            string commandName = typeof(TCommand).Name!;

            Result result = await innerHandler.HandleAsync(request, cancellationToken);

            Activity.Current?.AddTag("request.module", moduleName);
            Activity.Current?.AddTag("request.command", commandName);

            if (result.IsSuccess)
                logger.LogInformation("Finished command {Command} successfully", commandName);
            else
            {
                using (LogContext.PushProperty("Errors", result.Errors, true))
                {
                    logger.LogError("Finished command {Command} with errors", commandName);
                }
            }

            return result;
        }
    }

    internal sealed class QueryHandler<TQuery, TResponse>(
        ICommandHandler<TQuery, TResponse> innerHandler,
        ILogger<QueryHandler<TQuery, TResponse>> logger)
        : ICommandHandler<TQuery, TResponse>
        where TQuery : ICommand<TResponse>
    {
        public async Task<Result<TResponse>> HandleAsync(TQuery request, CancellationToken cancellationToken = default)
        {
            string moduleName = GetModuleName(typeof(TQuery).FullName!);
            string queryName = typeof(TQuery).Name!;

            Result<TResponse> result = await innerHandler.HandleAsync(request, cancellationToken);

            Activity.Current?.AddTag("request.module", moduleName);
            Activity.Current?.AddTag("request.query", queryName);

            if (result.IsSuccess)
                logger.LogInformation("Finished query {Query} successfully", queryName);
            else
            {
                using (LogContext.PushProperty("Errors", result.Errors, true))
                {
                    logger.LogError("Finished query {Query} with errors", queryName);
                }
            }

            return result;
        }
    }

    private static string GetModuleName(string requestName) => string.Join('.', requestName.Split('.')[..2]);
}
