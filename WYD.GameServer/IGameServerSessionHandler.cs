using WYD.Network;

namespace WYD.GameServer;

public interface IGameServerSessionHandler
{
    Task OnConnectedAsync(ISession session);
    Task OnDisconnectedAsync(ISession session);
    Task OnPacketReceivedAsync(ISession session, ReadOnlyMemory<byte> packetBuffer);
}