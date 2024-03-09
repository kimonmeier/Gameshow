namespace Gameshow.Shared.Events.Player;

/// <summary>
/// Dieses Event wird vom Server an alle Clients gesendet um aufzuzeigen, dass ein neuer Spieler gejoined ist
/// </summary>
public sealed class PlayerJoinedEvent : IRequest
{
    public required Guid PlayerId { get; init; }
    
    public required string Name { get; init; }
    
    public required string Link { get; init; }
}