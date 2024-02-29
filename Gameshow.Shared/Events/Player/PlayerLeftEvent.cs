namespace Gameshow.Shared.Events.Player;

public sealed class PlayerLeftEvent : IRequest
{
    public Guid PlayerId { get; init; }
    
}