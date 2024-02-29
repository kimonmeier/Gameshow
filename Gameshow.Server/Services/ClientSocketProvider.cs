using Fleck;

namespace Gameshow.Server.Services;

public sealed class ClientSocketProvider
{
    public IWebSocketConnection Client { get; set; }
    
}