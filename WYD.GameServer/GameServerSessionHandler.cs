using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WYD.Network;

namespace WYD.GameServer;

public class GameServerSessionHandler(ILoggerFactory loggerFactory, IServiceProvider serviceProvider, GameServerUserManager gameServerUserManager) : IGameServerSessionHandler
{
    private readonly ILogger<GameServerSessionHandler> _logger = loggerFactory.CreateLogger<GameServerSessionHandler>();
    private readonly SessionIdManager _sessionIdManager = new(1000);

    public async Task OnAcceptedAsync(ISession session)
    {
        if (!_sessionIdManager.HasAvailableSessionId)
        {
            _logger.LogInformation("No available session IDs. Disconnecting session...");
            await session.DisconnectAsync();
            return;
        }
        
        var id = _sessionIdManager.GetNextId();
        session.AssignSessionId(id);
        
        var user = new GameServerUser(session);

        if (!gameServerUserManager.AddUser(user))
        {
            _sessionIdManager.Release(id);
            
            await session.DisconnectAsync();
            return;
        }
        
        _ = Task.Run(session.RunAsync);
    }

    public Task OnDisconnectedAsync(ISession session)
    {
        var user = gameServerUserManager.GetUser(session.Id);
        if (user is null)
        {
            _logger.LogError("Disconnection attempt for an unregistered game server user. SessionId: {SessionId}", session.Id);
            return Task.CompletedTask;
        }
        
        gameServerUserManager.RemoveUser(user.Id);
        _sessionIdManager.Release(session.Id);
        
        return Task.CompletedTask;
    }

    public async Task OnPacketReceivedAsync(ISession session, ReadOnlyMemory<byte> packetBuffer)
    {
        var user = gameServerUserManager.GetUser(session.Id);
        if (user is null)
        {
            _logger.LogError("Packet received for an unregistered game server user. SessionId: {SessionId}", session.Id);
            return;
        }
        
        var incomingPacket = new IncomingPacket(packetBuffer);
        
        var packetCode = (GameIncomingPacketCode)incomingPacket.Code;
        if (!GameIncomingPacketHandlers.TryGet(packetCode, out var handlerType))
        {
            _logger.LogError("No packet handler found for packet code: {PacketCode}", packetCode);
            return;
        }

        if (serviceProvider.GetRequiredService(handlerType!) is not IGameServerPacketHandler packetHandler)
        {
            _logger.LogError("Failed to resolve service of type {HandlerName} for packet code: {PacketCode}", handlerType!.Name, packetCode);
            return;
        }
        
        await packetHandler.HandleAsync(user, incomingPacket);
    }
}