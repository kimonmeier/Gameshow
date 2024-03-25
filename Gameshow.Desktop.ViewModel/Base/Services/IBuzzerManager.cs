namespace Gameshow.Desktop.ViewModel.Base.Services;

public interface IBuzzerManager
{
    public bool IsLocked { get; }
    
    void ResetBuzzer();

    void BuzzerPressed();

    void SetPlayerBuzzed(Guid playerId);
}