using Gameshow.Shared.Events.Base;

namespace Gameshow.Server.Services;

public sealed class EventQueue
{
    private readonly Queue<IRequest> eventsProcessed = new();

    public void Enqueue(IRequest @event)
    {
        eventsProcessed.Enqueue(@event);
    }

    public List<IRequest> GetProcessedEvents()
    {
        return eventsProcessed.ToList();
    }
}