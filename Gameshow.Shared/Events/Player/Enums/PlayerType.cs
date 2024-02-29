namespace Gameshow.Shared.Events.Player.Enums;

/// <summary>
/// Dieser Enum beschreibt was für ein Typ Spieler der aktuell eingeloggte Spieler ist.
/// </summary>
public enum PlayerType : int
{
    /// <summary>
    /// Zuschauer
    /// Dieser Typ ist für das Twitch Overlay vorgesehen
    /// </summary>
    Spectator,
    /// <summary>
    /// Ein Spieler welcher am Spiel teilnimmt
    /// </summary>
    Player,
    /// <summary>
    /// Der Gamemaster welcher das Spiel steuert
    /// </summary>
    GameMaster
}