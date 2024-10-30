using WYD.Infrastructure;
using WYD.Network;

namespace WYD.GameServer;

public sealed class GameServerProtocol(IGameServerSessionHandler gameServerSessionHandler, ITaskProducer taskProducer) : IProtocol
{
    public Task OnConnectedAsync(ISession session)
    {
        taskProducer.Write(() => gameServerSessionHandler.OnConnectedAsync(session));
        return Task.CompletedTask;
    }

    public Task OnDisconnectedAsync(ISession session)
    {
        taskProducer.Write(() => gameServerSessionHandler.OnDisconnectedAsync(session));
        return Task.CompletedTask;
    }

    public Task OnPacketReceivedAsync(ISession session, ReadOnlyMemory<byte> packetBuffer)
    {
        taskProducer.Write(() => gameServerSessionHandler.OnPacketReceivedAsync(session, packetBuffer));
        return Task.CompletedTask;
    }
}
