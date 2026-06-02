using System.Diagnostics;
using Shared.Application.Messaging;
using Shared.Domain;

namespace Shared.Application.Behaviours;

internal static class LoggingDecorator
{
    internal sealed class CommandHandler<TCommand, TResponse>(
        ICommandHandler<TCommand, TResponse> innerHandler)
        : ICommandHandler<TCommand, TResponse>
        where TCommand : ICommand<TResponse>
    {
        public async Task<Result<TResponse>> HandleAsync(TCommand request, CancellationToken cancellationToken = default)
        {
            Result<TResponse> result = await innerHandler.HandleAsync(request, cancellationToken);

            Activity.Current?.AddTag("command", typeof(TCommand).FullName);
            if (result.IsSuccess)
            {
            }
            else
            {
            }


            return result;
        }
    }
}
