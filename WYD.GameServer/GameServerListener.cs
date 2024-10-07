using Microsoft.Extensions.Logging;
using WYD.Network;

namespace WYD.GameServer;

public sealed class GameServerListener : Listener
{
    public GameServerListener(string ipAddress, int port, IProtocol protocol, ILoggerFactory loggerFactory) 
        : base(ipAddress, port, protocol, loggerFactory)
    {
    }
}
