namespace Gameshow.Desktop.ViewModel.Component.GameMaster.General;

public class BuzzerInfoViewModel : BindableBase
{
    private readonly IPlayerManager playerManager;

    public BuzzerInfoViewModel(IPlayerManager playerManager)
    {
        this.playerManager = playerManager;
    }
}