using Gameshow.Shared.Events.Player.Enums;

namespace Gameshow.Shared.Events.Player;

public sealed class PlayerConnectingEvent : IRequest<Guid>
{
    public required string Name { get; init; }
    
    public required string Link { get; init; }
    
    public required PlayerType Type { get; init; }
}