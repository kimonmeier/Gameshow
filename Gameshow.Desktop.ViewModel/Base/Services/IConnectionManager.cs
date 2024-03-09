using MediatR;

namespace Gameshow.Desktop.ViewModel.Base.Services;

public interface IConnectionManager
{
    bool Connect();

    void Disconnect();

    void Send(IRequest request);

    TAnswer Send<TAnswer>(IRequest<TAnswer> request) where TAnswer : new();

    void RegisterEventHandler<TRequest>(Action handler) where TRequest : IBaseRequest;

    void UnregisterEventHandler<TRequest>(Action handler) where TRequest : IBaseRequest;
}