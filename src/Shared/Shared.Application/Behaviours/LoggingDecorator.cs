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

    private static string GetModuleName(string requestName) => string.Join('.', requestName.Split('.')[..2]);
}
