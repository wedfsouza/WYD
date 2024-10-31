using System.Collections.Frozen;
using WYD.GameServer.PacketHandlers;

namespace WYD.GameServer;

public static class GameIncomingPacketHandlers
{
    private static readonly FrozenDictionary<GameIncomingPacketCode, Type> PacketHandlers = new Dictionary<GameIncomingPacketCode, Type>()
    {
        { GameIncomingPacketCode.AccountLogin, typeof(AccountLoginPacketHandler) }
    }.ToFrozenDictionary();

    public static bool TryGet(GameIncomingPacketCode packetCode, out Type? packetHandlerType)
    {
        return PacketHandlers.TryGetValue(packetCode, out packetHandlerType);
    }

    public static FrozenDictionary<GameIncomingPacketCode, Type> GetAll()
    {
        return PacketHandlers;
    }
}