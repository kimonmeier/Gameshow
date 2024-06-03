using Gameshow.Desktop.ViewModel.Component.GameMaster.General;
using Microsoft.Extensions.DependencyInjection;

namespace Gameshow.Desktop.Services;

public sealed class BuzzerManager : IBuzzerManager
{
    public bool IsLocked { get; private set; } = true;

    public Guid? PlayerBuzzed { get; private set; }

    private readonly ConnectionManager connectionManager;
    private readonly IServiceProvider serviceProvider;
    private BuzzerInfoViewModel? buzzerInfoViewModel;
    private IPlayerManager? playerManager;
    private IPlayerScoreFactory? playerScoreFactory;

    private IPlayerManager PlayerManager
    {
        get
        {
            if (playerManager == null)
            {
                playerManager = serviceProvider.GetRequiredService<IPlayerManager>();
            }

            return playerManager;
        }
    }

    private IPlayerScoreFactory PlayerScoreFactory
    {
        get
        {
            if (playerScoreFactory == null)
            {
                playerScoreFactory = serviceProvider.GetRequiredService<IPlayerScoreFactory>();
            }

            return playerScoreFactory;
        }
    }

    private BuzzerInfoViewModel BuzzerInfoViewModel
    {
        get
        {
            if (buzzerInfoViewModel == null)
            {
                buzzerInfoViewModel = serviceProvider.GetRequiredService<BuzzerInfoViewModel>();
            }

            return buzzerInfoViewModel;
        }
    }

    public BuzzerManager(ConnectionManager connectionManager, IServiceProvider serviceProvider)
    {
        this.connectionManager = connectionManager;
        this.serviceProvider = serviceProvider;
    }

    public void ResetBuzzer()
    {
        IsLocked = false;
        PlayerBuzzed = null;
        Application.Current.Dispatcher.Invoke(delegate
        {
            foreach (Guid playerId in PlayerManager.Players)
            {
                PlayerScoreFactory.GetDetailsModel(playerId).IsBuzzerPressed = false;
            }

            BuzzerInfoViewModel.TellUiToUpdate();
        });
    }

    public void BuzzerPressed()
    {
        connectionManager.Send(new BuzzerPressedEvent()
        {
            PlayerId = PlayerManager.PlayerId
        });
    }

    public void SetPlayerBuzzed(Guid playerId)
    {
        Application.Current.Dispatcher.Invoke(delegate
        {
            PlayerScoreFactory.GetDetailsModel(playerId).IsBuzzerPressed = true;
            PlayerBuzzed = playerId;
        });

        BuzzerInfoViewModel.TellUiToUpdate();
    }
}