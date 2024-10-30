using WYD.Network;

namespace WYD.GameServer;

public class GameServerSessionHandler : IGameServerSessionHandler
{
    public Task OnConnectedAsync(ISession session)
    {
        return Task.CompletedTask;
    }

    public Task OnDisconnectedAsync(ISession session)
    {
        return Task.CompletedTask;
    }

    public Task OnPacketReceivedAsync(ISession session, ReadOnlyMemory<byte> packetBuffer)
    {
        return Task.CompletedTask;
    }
}