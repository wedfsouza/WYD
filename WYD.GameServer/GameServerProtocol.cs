using WYD.Network;

namespace WYD.GameServer;

public sealed class GameServerProtocol : IProtocol
{
    public Task OnConnectedAsync(ISession session)
    {
        return Task.CompletedTask;
    }

    public Task OnDisconnectedAsync(ISession session)
    {
        return Task.CompletedTask;
    }
}
