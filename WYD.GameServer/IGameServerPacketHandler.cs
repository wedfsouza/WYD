using WYD.Network;

namespace WYD.GameServer;

public interface IGameServerPacketHandler
{
    Task HandleAsync(GameServerUser user, IncomingPacket incomingPacket);
}