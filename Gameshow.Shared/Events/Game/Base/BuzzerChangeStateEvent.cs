namespace Gameshow.Shared.Events.Game.Base;

public sealed class BuzzerChangeStateEvent : IRequest
{
    public bool IsLocked { get; set; }
}