namespace Gameshow.Desktop.ViewModel.Base.Services;

public interface IPlayerScoreFactory
{
    UIElement? GetUiElement(Guid? playerId, ScoreType scoreType);
    
    void RegisterPlayer(PlayerInformation playerInformation);

    void RemovePlayer(Guid playerId);
}