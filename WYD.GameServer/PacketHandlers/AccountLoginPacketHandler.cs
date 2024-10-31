using WYD.Network;

namespace WYD.GameServer.PacketHandlers;

public class AccountLoginPacketHandler : IGameServerPacketHandler
{
    public Task HandleAsync(GameServerUser user, IncomingPacket incomingPacket)
    {
        var packet = new AccountLoginPacket(incomingPacket);
        
        return Task.CompletedTask;
    }
}