using Microsoft.Extensions.DependencyInjection;

namespace Gameshow.Desktop.Services;

public sealed class BuzzerManager : IBuzzerManager
{
    public bool IsLocked { get; private set; } = true;

    private readonly ConnectionManager connectionManager;
    private readonly IServiceProvider serviceProvider;
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

    public BuzzerManager(ConnectionManager connectionManager, IServiceProvider serviceProvider)
    {
        this.connectionManager = connectionManager;
        this.serviceProvider = serviceProvider;
    }

    public void ResetBuzzer()
    {
        IsLocked = false;

        foreach (Guid playerId in PlayerManager.Players)
        {
            PlayerScoreFactory.GetDetailsModel(playerId).IsBuzzerPressed = false;
        }
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
        PlayerScoreFactory.GetDetailsModel(playerId).IsBuzzerPressed = true;
    }
}