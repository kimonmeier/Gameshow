namespace Gameshow.Desktop.ViewModel.Base.Services;

public interface IPlayerScoreFactory
{
    event EventHandler<int> PlayerJoined;
    
    event EventHandler<int> PlayerLeft;

    event EventHandler<ScoreType> ScoreTypeChanged;
    
    BindableBase? GetUiModel(Guid? playerId, ScoreType scoreType);

    PlayerDetailsModel? GetByPlayerNumber(int number);

    PlayerDetailsModel GetDetailsModel(Guid playerId);
    
    void RegisterPlayer(PlayerInformation playerInformation);

    void RemovePlayer(Guid playerId);
}