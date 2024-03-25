namespace Gameshow.Desktop.ViewModel.Base.Services;

public interface IPlayerScoreFactory
{
    UIElement? GetUiElement(Guid? playerId, ScoreType scoreType);

    PlayerDetailsModel GetDetailsModel(Guid playerId);
    
    void RegisterPlayer(PlayerInformation playerInformation);

    void RemovePlayer(Guid playerId);
}