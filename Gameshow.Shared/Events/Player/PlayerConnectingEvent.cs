namespace Gameshow.Shared.Events.Player;

public sealed class PlayerConnectingEvent : IRequest<Guid>
{
    public string? Name { get; init; }
    
    public string? Link { get; init; }
    
    public required PlayerType Type { get; init; }
}