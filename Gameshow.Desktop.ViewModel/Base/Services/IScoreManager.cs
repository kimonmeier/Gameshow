namespace Gameshow.Desktop.ViewModel.Base.Services;

public interface IScoreManager
{
    void ResetPoints();

    void AddPoint(Guid playerId);

    void RemovePoint(Guid playerId);
}