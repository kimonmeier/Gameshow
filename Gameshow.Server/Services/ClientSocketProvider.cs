namespace Gameshow.Server.Services;

public sealed class ClientSocketProvider
{
    public required IWebSocketConnection Client { get; set; }
    
}