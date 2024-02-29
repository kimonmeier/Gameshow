using Fleck;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gameshow.Server.Services
{
    public interface IWebsocketManager
    {
        event EventHandler<IWebSocketConnection> ClientConnected;

        event EventHandler<IWebSocketConnection> ClientDisconnected;

        void SendMessage(IWebSocketConnection client, IRequest request);

        void SendMessage(IRequest request);

        TAnswer? SendMessage<TAnswer>(IWebSocketConnection client, IRequest<TAnswer> request) where TAnswer : class;

        void RecievedAnswer(Guid eventGuid, dynamic? result);
    }
}
