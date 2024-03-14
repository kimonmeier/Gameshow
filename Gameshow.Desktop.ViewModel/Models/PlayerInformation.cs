namespace Gameshow.Desktop.ViewModel.Models;

public class PlayerInformation
{
    public required Guid PlayerId { get; set; }
        
    public required string Name { get; set; }
        
    public required string Link { get; set; }
}