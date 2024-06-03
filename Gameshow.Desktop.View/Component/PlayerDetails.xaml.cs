using Gameshow.Desktop.ViewModel.Base.Services;
using Gameshow.Desktop.ViewModel.Component.Player;
using Gameshow.Shared.Models.Score;

namespace Gameshow.Desktop.View.Component;

public partial class PlayerDetails
{
    public int Player { get; set; }

    private readonly IPlayerScoreFactory playerScoreFactory;

    public PlayerDetails()
    {
        playerScoreFactory = DependencyInjectionLocator.Provider.GetRequiredService<IPlayerScoreFactory>();
        playerScoreFactory.PlayerJoined += (sender, i) =>
        {
            if (Player != i)
            {
                return;
            }

            PlayerDetailsModel model = playerScoreFactory.GetByPlayerNumber(Player)!;
            PlayerName.OpenPlayer(model.PlayerId);
            DataContext = model;
        };

        playerScoreFactory.PlayerLeft += (sender, i) =>
        {
            if (Player != i)
            {
                return;
            }

            DataContext = null;
        };

        playerScoreFactory.ScoreTypeChanged += PlayerScoreFactoryOnScoreTypeChanged;

        InitializeComponent();
    }

    private void PlayerScoreFactoryOnScoreTypeChanged(object? sender, ScoreType e)
    {
        PlayerName.Visibility = Visibility.Hidden;

        switch (e)
        {
            
            case ScoreType.None:
            default:
                PlayerName.Visibility = Visibility.Visible;
                break;
        }
    }
}