namespace Gameshow.Desktop.ViewModel.Component.Player;

public interface IPlayerPointModel
{
    Guid PlayerId { get; set; }
    
    int Points { get; set; }
}