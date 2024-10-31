using WYD.Network;

namespace WYD.GameServer.PacketHandlers;

public class AccountLoginPacket
{
    public AccountLoginPacket(IncomingPacket packet)
    {
        Password = packet.ReadFixedString(16);
        Username = packet.ReadFixedString(16);
        Tid = packet.ReadFixedString(52);
        Version = packet.ReadInt32();
        Force = packet.ReadInt32();
    }

    public string Username { get; }
    public string Password { get; }
    public string Tid { get; }
    public int Version { get; }
    public int Force { get; }
}