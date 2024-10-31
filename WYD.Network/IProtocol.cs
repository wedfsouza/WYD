namespace WYD.Network;

public interface IProtocol
{
    Task OnAcceptedAsync(ISession session);
    Task OnConnectedAsync(ISession session);
    Task OnDisconnectedAsync(ISession session);
    Task OnPacketReceivedAsync(ISession session, ReadOnlyMemory<byte> packetBuffer);
}
