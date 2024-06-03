namespace Gameshow.Desktop.ViewModel.Component.GameMaster.General;

public class BuzzerInfoViewModel : BindableBase
{
    private readonly IPlayerScoreFactory playerScoreFactory;
    private readonly IBuzzerManager buzzerManager;

    public BuzzerInfoViewModel(IBuzzerManager buzzerManager, IPlayerScoreFactory playerScoreFactory, BuzzerInfoResetBuzzerCommand buzzerInfoResetBuzzerCommand)
    {
        this.buzzerManager = buzzerManager;
        this.playerScoreFactory = playerScoreFactory;
        ResetBuzzerCommand = buzzerInfoResetBuzzerCommand;
    }

    public bool BuzzerPressed => buzzerManager.IsLocked || buzzerManager.PlayerBuzzed != null;

    public string BuzzerPressedName
    {
        get
        {
            if (buzzerManager.IsLocked)
            {
                return "Buzzer ist gelocked";
            }
            
            if (buzzerManager.PlayerBuzzed is null)
            {
                return "Nicht gedrückt";
            }

            PlayerDetailsModel detailsModel = playerScoreFactory.GetDetailsModel(buzzerManager.PlayerBuzzed.Value);

            if (detailsModel.Name is null)
            {
                return "Unbekannter Spieler gedrückt!";
            }

            return $"Spieler {detailsModel.Name} hat gedrückt!";
        }
    }

    public ICommand ResetBuzzerCommand { get; init; }

    public void TellUiToUpdate()
    {
        OnPropertyChanged(nameof(BuzzerPressed));
        OnPropertyChanged(nameof(BuzzerPressedName));
    }
}