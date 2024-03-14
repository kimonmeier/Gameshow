using Gameshow.Shared.Events.Player.Enums;

namespace Gameshow.Desktop.Services;

public sealed class GameManager : IGameManager
{
    public PlayerType PlayerType { get; set; }
}