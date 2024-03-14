using Gameshow.Desktop.ViewModel.Base;
using Gameshow.Desktop.ViewModel.Base.Services;

namespace Gameshow.Desktop.ViewModel.Component;

public sealed class PlayerDetailsModel : BindableBase
{
    private string? name;
    private string? url;
    private ScoreType scoreType;
    private UIElement? scoreElement;
    private Guid? playerGuid;

    private readonly IPlayerScoreFactory playerDetailsFactory;

    public PlayerDetailsModel(IPlayerScoreFactory playerDetailsFactory, PlayerInformation playerInformation)
    {
        this.playerDetailsFactory = playerDetailsFactory;
        playerGuid = playerInformation.PlayerId;
        name = playerInformation.Name;
        url = playerInformation.Link;
    }

    [Obsolete("Just for Designer")]
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public PlayerDetailsModel()
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    {
        
    }

    public ScoreType ScoreType
    {
        get => scoreType;
        set
        {
            scoreType = value;
            OnPropertyChanged();
            ScoreElement = playerDetailsFactory.GetUiElement(playerGuid, value);
        }
    }

    public string? Url
    {
        get => url;
        set
        {
            url = value;
            OnPropertyChanged();
        }
    }

    public string? Name
    {
        get => name;
        set
        {
            name = value;
            OnPropertyChanged();
        }
    }

    public UIElement? ScoreElement
    {
        get => scoreElement;
        set
        {
            scoreElement = value;
            OnPropertyChanged();
        }
    }
}