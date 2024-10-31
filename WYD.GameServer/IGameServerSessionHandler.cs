using WYD.Network;

namespace WYD.GameServer;

public interface IGameServerSessionHandler
{
    Task OnAcceptedAsync(ISession session);
    Task OnDisconnectedAsync(ISession session);
    Task OnPacketReceivedAsync(ISession session, ReadOnlyMemory<byte> packetBuffer);
}