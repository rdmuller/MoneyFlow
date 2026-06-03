using System.Diagnostics;
using Microsoft.Extensions.Logging;
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
            Result<TResponse> result = await innerHandler.HandleAsync(request, cancellationToken);

            Activity.Current?.AddTag("command", typeof(TCommand).FullName);

            if (result.IsSuccess)
                logger.LogInformation("Finished command {Command} successfully", typeof(TCommand).FullName);
            else
                logger.LogError("Finished command {Command} with error", typeof(TCommand).FullName);

            return result;
        }
    }
}
