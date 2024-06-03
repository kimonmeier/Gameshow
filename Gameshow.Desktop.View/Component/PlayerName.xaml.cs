using Gameshow.Desktop.ViewModel.Base.Services;
using Gameshow.Shared.Models.Score;

namespace Gameshow.Desktop.View.Component;

public partial class PlayerName : UserControl
{
    public PlayerName()
    {
        InitializeComponent();
    }

    public void OpenPlayer(Guid playerId)
    {
        DataContext = DependencyInjectionLocator.Provider.GetRequiredService<IPlayerScoreFactory>().GetUiModel(playerId, ScoreType.None);
        this.InvalidateProperty(DataContextProperty);
    }
}