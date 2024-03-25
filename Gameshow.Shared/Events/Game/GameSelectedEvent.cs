namespace Gameshow.Shared.Events.Game;

public class GameSelectedEvent : IRequest
{
    public GameType Game { get; set; }
}