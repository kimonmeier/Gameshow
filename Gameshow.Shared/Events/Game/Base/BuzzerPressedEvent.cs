namespace Gameshow.Shared.Events.Game.Base;

public sealed class BuzzerPressedEvent : IRequest
{
    public required Guid PlayerId { get; set; }
}