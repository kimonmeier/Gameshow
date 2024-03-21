using MediatR.Pipeline;
using Microsoft.Extensions.Logging;

namespace Gameshow.Shared.Services;

public class ExceptionLoggingHandler<TRequest, TResponse, TException> : IRequestExceptionHandler<TRequest, TResponse, TException>
    where TRequest : IRequest<TResponse>
    where TException : Exception
{
    private readonly ILogger<ExceptionLoggingHandler<TRequest, TResponse, TException>> _logger;

    public ExceptionLoggingHandler(ILogger<ExceptionLoggingHandler<TRequest, TResponse, TException>> logger)
    {
        _logger = logger;
    }

    public Task Handle(TRequest request, TException exception, RequestExceptionHandlerState<TResponse> state, CancellationToken cancellationToken)
    {
        _logger.LogError(exception, "Something went wrong while handling request of type {@requestType}", typeof(TRequest));

        return Task.CompletedTask;
    }
}