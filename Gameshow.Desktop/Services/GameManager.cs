using Gameshow.Shared.Events.Player.Enums;
using Gameshow.Shared.Models.Game;

namespace Gameshow.Desktop.Services;

public sealed class GameManager : IGameManager
{
    public PlayerType PlayerType { get; set; }

    public GameState GameState => GameState.Lobby;
}