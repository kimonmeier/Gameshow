namespace Gameshow.Shared.Events.Player;

/// <summary>
/// Dieses Event wird vom Server an alle Clients gesendet um aufzuzeigen, dass ein neuer Spieler gejoined ist
/// </summary>
public sealed class PlayerJoinedEvent : IRequest
{
    public Guid PlayerId { get; init; }
    
    public string Name { get; init; }
    
    public string Link { get; init; }
}