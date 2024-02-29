namespace Gameshow.Shared.Events.Player;

public sealed class PlayerDisconnectingEvent : IRequest
{
    public required Guid PlayerId { get; init; }
    
}